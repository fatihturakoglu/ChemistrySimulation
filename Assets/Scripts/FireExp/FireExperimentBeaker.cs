using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FireExperimentBeaker : MonoBehaviour
{
    public static FireExperimentBeaker Instance { get; private set; }

    [SerializeField] private SpiritLamp spiritLamp;
    [SerializeField] private FireExperimentSelection selectionManager;
    [SerializeField] private List<RecipeSO> allRecipes;

    private List<LabObject> labObjects = new List<LabObject>();
    private bool isReactionActive = false; // Reaksiyonun tekrar tekrar doğmasını engeller

    private void Awake() { Instance = this; }

    private void Start()
    {
        // KURAL 1: Lamba yakıldığında kontrol et
        spiritLamp.OnLightAction += (s, e) => CheckRecipes();

        // KURAL 2: Madde eklendiğinde, eğer ocak ZATEN yanıyorsa yine kontrol et
        selectionManager.OnIngredientAdded += (s, e) => {
            AddIngredient(e.labObject);

            // EĞER lamba zaten yanıyorsa, madde girer girmez reaksiyonu başlat
            if (spiritLamp.IsLit())
            {
                CheckRecipes();
            }
            else
            {
                Debug.Log("Madde eklendi ama lamba yanmadığı için bekliyor.");
            }
        };
    }

    private void AddIngredient(LabObject labObject)
    {
        if (!labObjects.Contains(labObject)) labObjects.Add(labObject);
    }

    public void CheckRecipes()
    {
        // Eğer ocak yanmıyorsa (kapatıldıysa) veya zaten duman varsa çık
        if (!spiritLamp.IsLit() || isReactionActive) return;

        List<LabObjectSO> currentSOs = labObjects.Select(x => x.GetLabObjectSO()).ToList();

        foreach (RecipeSO recipe in allRecipes)
        {
            bool hasAll = recipe.requiredIngredients.All(req => currentSOs.Any(curr => curr.objectName == req.objectName));
            bool countMatch = currentSOs.Count == recipe.requiredIngredients.Count;

            // Hem maddeler doğru hem de şu an ateş YENİ yakıldıysa başlat
            if (hasAll && countMatch)
            {
                StartReaction(recipe);
                break;
            }
        }
    }

    private void StartReaction(RecipeSO recipe)
    {
        isReactionActive = true;
        Debug.Log("Isı ve Malzeme Tamam: " + recipe.recipeName);

        if (recipe.reaction != null)
        {
            // Efekti beherin biraz üzerinde yarat
            GameObject fx = Instantiate(recipe.reaction, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            fx.transform.SetParent(this.transform);
        }

        // Listeyi hemen değil, reaksiyon scripti malzemeyi bulduktan sonra temizle
        StartCoroutine(ClearListDelayed());
    }

    private IEnumerator ClearListDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        labObjects.Clear();
        isReactionActive = false; // Yeni deney yapılabilmesi için kilidi aç
        Debug.Log("Beher temizlendi, yeni deneye hazır.");
    }

    public Transform GetFirstIngredient() => labObjects.Count > 0 ? labObjects[0].transform : null;
}