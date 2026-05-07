using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BeakerManager : MonoBehaviour, IBeaker
{
    [SerializeField] private SelectionManager ingredientManager;
    [SerializeField] private List<RecipeSO> allRecipes;

    [Header("Görsel Referanslar")]
    [SerializeField] private SilverMirrorVisualizer visualizer;

    private DensityTowerVisualizer _towerVis;
    private LiquidMixerVisualizer _mixerVis;
    private ColorMixerService _colorMixer;
    private IObjectResolver _container;

    private List<LabObjectSO> _liquidDataList = new List<LabObjectSO>();
    private List<LabObject> _allIngredients = new List<LabObject>();
    private Dictionary<string, int> _dropCounts = new Dictionary<string, int>();
    private List<string> _addedOrderNames = new List<string>();

    private Color _currentMixedColor = Color.white;

    [Inject]
    public void Construct(ColorMixerService colorMixer, IObjectResolver container)
    {
        _colorMixer = colorMixer;
        _container = container;
    }

    private void Start()
    {
        if (visualizer == null) visualizer = GetComponent<SilverMirrorVisualizer>();
        _towerVis = GetComponent<DensityTowerVisualizer>();
        _mixerVis = GetComponent<LiquidMixerVisualizer>();

        if (ingredientManager != null)
            ingredientManager.OnIngredientAdded += (s, e) => AddIngredient(e.labObject);

        if (visualizer != null)
        {
            if (visualizer.brownPrecipitate != null) visualizer.brownPrecipitate.SetActive(false);
            if (visualizer.liquidRenderer != null)
            {
                visualizer.liquidRenderer.gameObject.SetActive(false);
                visualizer.liquidRenderer.transform.localScale = new Vector3(1, 0, 1);
                _currentMixedColor = visualizer.liquidRenderer.material.color;
            }
        }

        if (SpiritLamp.Instance != null)
        {
            SpiritLamp.Instance.OnLightAction += (s, e) => TryStartMirrorReaction();
        }
    }

    public void AddIngredient(LabObject labObject, float animationDuration = 1.5f)
    {
        if (labObject == null) return;
        LabObjectSO so = labObject.GetLabObjectSO();

        if (so.isLiquid)
        {
            // KONTROL: Eğer bu spesifik LabObject zaten eklenmişse tekrar ekleme 
            // (Veya çok hızlı tıklamalarda aynı tür sıvının çift eklenmesini önlemek için)
            if (_allIngredients.Contains(labObject)) return;

            _liquidDataList.Add(so);
            _addedOrderNames.Add(so.objectName);
            _allIngredients.Add(labObject);

            StartParallelRise(so, animationDuration);
            UpdateColorVisuals(so, animationDuration);
        }
        else
        {
            // Katı maddeler için mantık
            if (!_allIngredients.Contains(labObject))
            {
                _addedOrderNames.Add(so.objectName);
                _allIngredients.Add(labObject);
            }
        }

        if (!so.isLiquid && !so.isReusable) HandleChemicalDissolution(labObject, so);
        HandleSilverMirrorVisuals(so.objectName);
        CheckRecipes(so.objectName);
    }

    public void StartParallelRise(LabObjectSO so, float duration)
    {
        // Yoğunluk kulesi mi yoksa gümüş aynası sahnesi mi ayrımı
        if (_towerVis != null)
        {
            // Yoğunluk kulesinde MainLiquid (Gümüş aynası sıvısı) KAPALI olmalı
            if (visualizer != null && visualizer.liquidRenderer != null)
                visualizer.liquidRenderer.gameObject.SetActive(false);

            _towerVis.UpdateDensityVisuals(_liquidDataList, duration);
        }
        else if (visualizer != null)
        {
            visualizer.PlayParallelRise(so, duration);
        }
    }

    private void UpdateColorVisuals(LabObjectSO addedSO, float duration)
    {
        // Kule sahnesinde renk karışımı olmaz
        if (_towerVis != null || visualizer == null || visualizer.liquidRenderer == null) return;

        if (!visualizer.liquidRenderer.gameObject.activeSelf)
            visualizer.liquidRenderer.gameObject.SetActive(true);

        if (_liquidDataList.Count == 1)
            _currentMixedColor = addedSO.color;
        else
            _currentMixedColor = Color.Lerp(_currentMixedColor, addedSO.color, addedSO.colorIntensity);

        visualizer.liquidRenderer.material.DOColor(_currentMixedColor, duration).SetEase(Ease.Linear);
    }

    private void HandleChemicalDissolution(LabObject obj, LabObjectSO so)
    {
        if (obj == null) return;

        // KONTROL: Çözücü listesinde uygun sıvı var mı?
        bool canDissolve = _liquidDataList.Any(liq =>
            so.solubleIn == null || so.solubleIn.Count == 0 ||
            so.solubleIn.Any(s => s.Equals(liq.objectName, StringComparison.OrdinalIgnoreCase)));

        if (canDissolve)
        {
            // Önceki animasyonları durdur
            obj.transform.DOKill();

            // 1. Katı madde (NaOH vb.) çözünürken küçülerek yok olur
            obj.transform.DOScale(Vector3.zero, 3.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                // 2. İSİM KONTROLÜ: Boşlukları temizleyerek kontrol et
                if (so.objectName.Trim() == "SodiumHydroxide")
                {
                    // 3. DENEY SIRALAMASI: Kapta Gümüş Nitrat var mı?
                    if (_addedOrderNames.Contains("SilverNitrate"))
                    {
                        // 4. GÖRSEL TETİKLEME: Ölçekle oynamadan sadece SetActive(true) yapar
                        if (visualizer != null)
                        {
                            visualizer.ShowBrownCloud();
                            Debug.Log("<color=brown>KİMYA:</color> Gümüş oksit çökeltisi (kahverengi) oluştu!");
                        }
                    }
                }

                // 5. BELLEK YÖNETİMİ: Çözünen objeyi sahneden sil
                if (obj != null) Destroy(obj.gameObject);
            });
        }
    }   

    // TEK BİR METOT OLARAK BIRAKILDI (Hata giderildi)
    private void HandleSilverMirrorVisuals(string addedName)
    {
        if (visualizer == null) return;

        // Amonyak bir sıvı olduğu için döküldüğü an berraklaştırma sürecini başlatır
        if (addedName == "AmmoniaSolution" && _addedOrderNames.Contains("SodiumHydroxide"))
        {
            visualizer.ClearCloud();
        }
    }


    public List<LabObjectSO> GetLiquidData()
    {
        return _liquidDataList;
    }

    private void TryStartMirrorReaction()
{
    // 1. KONTROL: Kapta Glikoz var mı?
    if (_addedOrderNames.Contains("GlucoseSolution"))
    {
        Debug.Log("<color=orange>OCAK YANDI:</color> Glikoz bulundu, tarif kontrol ediliyor...");
        
        // "Heated" parametresini malzeme listesine eklemeden direkt tarif döngüsüne giriyoruz
        ForceCheckSilverMirror(); 
    }
    else
    {
        Debug.LogWarning("Ocağı yaktın ama kapta Glikoz yok!");
    }
}

private void ForceCheckSilverMirror()
{
    // Gümüş aynası tarifini listeden bulup zorla çalıştıralım
    var mirrorRecipe = allRecipes.FirstOrDefault(r => r.recipeName == "SilverMirror");

    if (mirrorRecipe != null)
    {
        // Malzeme listesini al
        var currentSOs = _allIngredients.Select(x => x.GetLabObjectSO()).ToList();
        
        // Eksik malzeme var mı kontrol et
        bool hasAll = !mirrorRecipe.requiredIngredients.Except(currentSOs).Any();

        if (hasAll)
        {
            Debug.Log("<color=silver>BAŞARILI:</color> Gümüş aynası instantiate ediliyor!");
            _container.Instantiate(mirrorRecipe.reaction, transform.position + Vector3.up * 0.15f, Quaternion.identity);
            
            // Ayna oluştuğu için beheri SIFIRLAMIYORUZ (ResetBeaker çağırma!)
        }
        else
        {
            Debug.LogError("Gümüş aynası için malzemeler eksik!");
        }
    }
}

    private void CheckRecipes(string addedName)
    {
        // 1. ÖN HAZIRLIK: Sahneden silinmiş objeleri listeden temizle ve verileri çek
        _allIngredients.RemoveAll(item => item == null);
        var currentSOs = _allIngredients.Select(x => x.GetLabObjectSO()).ToList();

        foreach (var recipe in allRecipes)
        {
            // 2. TEMEL İÇERİK KONTROLÜ
            // Kapta gereken tüm malzemeler (en az gereken miktarda) var mı?
            bool hasAllIngredients = !recipe.requiredIngredients.Except(currentSOs).Any();
            bool hasCorrectCount = recipe.requiredIngredients.Count <= currentSOs.Count;

            if (hasAllIngredients && hasCorrectCount)
            {
                // 3. ÖZEL ŞART: SIRALAMA KONTROLÜ
                if (recipe.requiresSpecificOrder)
                {
                    bool orderCorrect = true;
                    for (int i = 0; i < recipe.requiredIngredients.Count; i++)
                    {
                        // Eklenen isim listesi ile tarifteki isimleri karşılaştır
                        if (_addedOrderNames.Count <= i || _addedOrderNames[i] != recipe.requiredIngredients[i].objectName)
                        {
                            orderCorrect = false;
                            break;
                        }
                    }
                    if (!orderCorrect) continue; // Sıralama yanlışsa bu tarifi pas geç
                }

                // 4. ÖZEL ŞART: DAMLA SAYISI KONTROLÜ
                if (recipe.requiresSpecificDropCount)
                {
                    if (!_dropCounts.ContainsKey(recipe.dropTargetName) || _dropCounts[recipe.dropTargetName] < recipe.requiredDropCount)
                    {
                        continue; // Damla sayısı yetersizse pas geç
                    }
                }

                // --- REAKSİYON BAŞLANGICI ---

                // Reaksiyon prefabını (gaz, patlama, ayna efekti vb.) oluştur
                if (recipe.reaction != null)
                {
                    _container.Instantiate(recipe.reaction, transform.position + Vector3.up * 0.15f, Quaternion.identity);
                    Debug.Log($"<color=cyan>DENEY:</color> {recipe.recipeName} başarıyla tetiklendi!");
                }

                // DÖNGÜDEN ÇIK: Bir reaksiyon tetiklendikten sonra aynı anda başkası tetiklenmesin
                break;
            }
        }
    }

    // SelectionManager'ın kuleyi tetikleyebilmesi için eklediğimiz basit metot
    public void TriggerTowerVisuals()
    {
        if (_towerVis != null)
        {
            _towerVis.UpdateDensityVisuals(_liquidDataList);
        }
    }

    public void ResetBeaker()
    {
        if (visualizer != null && visualizer.liquidRenderer != null)
        {
            visualizer.liquidRenderer.transform.DOKill();
            visualizer.liquidRenderer.material.DOKill();
        }

        _allIngredients.Clear();
        _liquidDataList.Clear();
        _colorMixer.Clear();
        _dropCounts.Clear();
        _addedOrderNames.Clear();
        _currentMixedColor = Color.white;

        if (visualizer != null)
        {
            if (visualizer.brownPrecipitate != null) visualizer.brownPrecipitate.SetActive(false);
            if (visualizer.liquidRenderer != null)
                visualizer.liquidRenderer.transform.DOScaleY(0, 1f).OnComplete(() => visualizer.liquidRenderer.gameObject.SetActive(false));
        }
    }

    public MeshRenderer MainLiquid => visualizer != null ? visualizer.liquidRenderer : GetComponentInChildren<MeshRenderer>();
    public MeshRenderer[] LayeredLiquids => GetComponentsInChildren<MeshRenderer>();
    public Vector3 ReactionPosition => transform.position + Vector3.up * 0.15f;
}