using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Collections.Generic;
using System.Linq;
using System;

public class BeakerManager : MonoBehaviour, IBeaker
{
    [SerializeField] private SelectionManager ingredientManager;
    [SerializeField] private List<RecipeSO> allRecipes;

    private DensityTowerVisualizer _towerVis;
    private LiquidMixerVisualizer _mixerVis;
    private ColorMixerService _colorMixer;
    private IObjectResolver _container;

    private List<LabObjectSO> _liquidDataList = new List<LabObjectSO>();
    private List<LabObject> _allIngredients = new List<LabObject>();

    // --- ASİT-BAZ ÖZEL DEĞİŞKENLERİ GERİ GELDİ ---
    private int _baseDrops = 0;

    [Inject]
    public void Construct(ColorMixerService colorMixer, IObjectResolver container)
    {
        _colorMixer = colorMixer;
        _container = container;
    }

    private void Start()
    {
        _towerVis = GetComponent<DensityTowerVisualizer>();
        _mixerVis = GetComponent<LiquidMixerVisualizer>();

        if (ingredientManager != null)
            ingredientManager.OnIngredientAdded += (s, e) => AddIngredient(e.labObject);
    }

    public void AddIngredient(LabObject labObject)
    {
        LabObjectSO so = labObject.GetLabObjectSO();

        // Asit-Baz kontrolü için asit var mı bakıyoruz
        bool alreadyHasAcid = _allIngredients.Any(x => x.GetLabObjectSO().objectName == "Asit");

        // 1. LİSTE GÜNCELLEME (Asit-Baz özel mantığıyla)
        UpdateIngredientLists(labObject, so, alreadyHasAcid);

        // 2. GÖRSEL GÜNCELLEME
        if (so.isLiquid)
        {
            _liquidDataList.Add(so);

            if (_towerVis != null)
                _towerVis.UpdateDensityVisuals(_liquidDataList);
            else if (_mixerVis != null)
            {
                _colorMixer.AddColor(so.color);
                _mixerVis.UpdateMixedVisuals(_colorMixer.GetMixedColor(), _liquidDataList.Count);
            }
        }

        // 3. REÇETE KONTROLÜ
        CheckRecipes(so.objectName, alreadyHasAcid);
    }

    // --- ESKİ MANTIK: LİSTE YÖNETİMİ ---
    private void UpdateIngredientLists(LabObject labObject, LabObjectSO so, bool hasAcid)
    {
        if (so.objectName == "Asit" && !hasAcid)
        {
            _allIngredients.Add(labObject);
        }
        else if (so.objectName == "Baz")
        {
            // Bazı listeye sadece bir kere ekle ama damla sayısını artır
            if (!_allIngredients.Any(x => x.GetLabObjectSO().objectName == "Baz"))
                _allIngredients.Add(labObject);

            _baseDrops++;
            Debug.Log("Baz Damlası: " + _baseDrops);
        }
        else if (!_allIngredients.Contains(labObject))
        {
            _allIngredients.Add(labObject);
        }
    }

    private void CheckRecipes(string addedName, bool hasAcid)
    {
        // --- ASİT-BAZ ÖZEL FİLTRESİ ---
        // Eğer baz ekleniyorsa ve bu bir asit-baz deneyi ise (ortamda asit varsa) 5 damla kuralını işlet.
        // Ama Sodyum-Su gibi deneylerde "Asit" yoksa bu filtreyi bypass etmeliyiz.
        if (addedName == "Baz" && hasAcid)
        {
            if (_baseDrops < 5) return;
        }

        // Sadece asit eklendiğinde tepkime hemen başlamasın (Asit-Baz deneyi koruması)
        if (addedName == "Asit") return;

        // --- GENEL REÇETE SİSTEMİ ---
        var currentSOs = _allIngredients.Select(x => x.GetLabObjectSO()).ToList();

        foreach (var recipe in allRecipes)
        {
            // Reçetedeki malzemeler ile beherin içindekiler tam uyuşuyor mu?
            bool match = !recipe.requiredIngredients.Except(currentSOs).Any() &&
                         currentSOs.Count == recipe.requiredIngredients.Count;

            if (match)
            {
                Debug.Log("Reçete Tamamlandı: " + recipe.recipeName);
                _container.Instantiate(recipe.reaction, transform.position + Vector3.up * 0.15f, Quaternion.identity);
                ResetBeaker();
                break;
            }
        }
    }

    public void ResetBeaker()
    {
        _allIngredients.Clear();
        _liquidDataList.Clear();
        _colorMixer.Clear();
        _baseDrops = 0; // Damla sayısını sıfırla

        if (_towerVis) _towerVis.UpdateDensityVisuals(_liquidDataList);
        if (_mixerVis) _mixerVis.UpdateMixedVisuals(Color.white, 0);
    }

    public MeshRenderer MainLiquid => GetComponentInChildren<MeshRenderer>();
    public MeshRenderer[] LayeredLiquids => GetComponentsInChildren<MeshRenderer>();
    public Vector3 ReactionPosition => transform.position + Vector3.up * 0.15f;
}