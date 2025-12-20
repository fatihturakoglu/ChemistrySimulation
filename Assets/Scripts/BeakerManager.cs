using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeakerManager : MonoBehaviour {
    public static BeakerManager Instance { get; private set; }

    [SerializeField] private SelectionManager ingredientManager;
    [SerializeField] private List<RecipeSO> allRecipes;
    [SerializeField] private MeshRenderer mainLiquidMesh;

    private bool isReactionPerforming;
    private float reactPerformTime = .5f;
    private RecipeSO currentRecipe;

    private List<LabObject> labObjects;

    private void ÝngredientManager_OnIngredientAdded(object sender, SelectionManager.OnIngredientAddedEventArgs e) {
        AddIngredient(e.labObject);
        Debug.Log(e.labObject.GetLabObjectSO().objectName + " þuan beherde mevcut.");
    }

    private void Start() {
        ingredientManager.OnIngredientAdded += ÝngredientManager_OnIngredientAdded;
        labObjects = new List<LabObject>();

        if(Instance != null) {
            Debug.LogError("Birden fazla BeakerManager nesnesi var!");
        }
        Instance = this;
    }

    private void Update() {
        HandleReaction();
    }

    private void CheckRecipes() {
        List<LabObjectSO> currentLabObjectsSO = new List<LabObjectSO>();

        foreach (LabObject labObject in labObjects) {
            currentLabObjectsSO.Add(labObject.GetLabObjectSO());
        }

        foreach (RecipeSO recipe in allRecipes) {
            // LINQ Logic:
            // 1. Is there any ingredient in the recipe that is NOT in the beaker?
            bool missingIngredients = recipe.requiredIngredients.Except(currentLabObjectsSO).Any();

            // 2. Is there any ingredient in the beaker that is NOT in the recipe?
            bool extraIngredients = currentLabObjectsSO.Except(recipe.requiredIngredients).Any();

            // Check if lists are identical (no missing, no extra, same count)
            if (!missingIngredients && !extraIngredients && currentLabObjectsSO.Count == recipe.requiredIngredients.Count) {
                //PerformReaction(recipe); tepkime süresini beklemek istiyoruz, o yüzden bool ile aktif edeceðiz
                StartReaction(recipe);
                return; // Stop checking after finding a match
            }
        }
    }

    private void StartReaction(RecipeSO recipe) {
        currentRecipe = recipe;
        isReactionPerforming = true;
    }

    private void StopReaction() {
        currentRecipe = null;
        isReactionPerforming = false;
    }

    private void HandleReaction() {
        if (isReactionPerforming) {
            reactPerformTime -= Time.deltaTime;
            if (reactPerformTime <= 0 && currentRecipe != null) {
                PerformReaction(currentRecipe);
            }
        }
    }

    private void PerformReaction(RecipeSO recipe) {
        Debug.Log("Tepkime baþladý: " + recipe.recipeName);
        float yOffset = 0.11f;
        Vector3 reactionPos = transform.position;
        reactionPos.y += yOffset;

        if (recipe.reaction != null) {
            Instantiate(recipe.reaction, reactionPos, Quaternion.identity);
        }

        ResetBeaker();
        StopReaction();
    }

    private void ResetBeaker() {
        foreach (LabObject ingredient in labObjects) {
            if (!ingredient.GetLabObjectSO().isReusable)
                Destroy(ingredient.gameObject); //tekrar kullanýlmayan maddeler yok olsun
        }

        ResetLiquid();
        labObjects.Clear(); //tepkime olunca beherdeki tüm maddeler sýfýrlansýn
    }

    private void AddIngredient(LabObject labObject) {
        if(!labObjects.Contains(labObject)) //ayný malzemeden iki defa koymasýn
            labObjects.Add(labObject);

        bool isLiquid = labObject.GetLabObjectSO().isLiquid; //sudan baþka bir sývý eklenecekse metot deðiþmeli
        if (isLiquid) 
            HandleLiquids(labObject);

        CheckRecipes();
    }

    private void HandleLiquids(LabObject labObject) {
        var liquidColor = labObject.GetLabObjectSO().color;

        mainLiquidMesh.material.color = liquidColor;
        mainLiquidMesh.gameObject.SetActive(true);
    }

    private void ResetLiquid() { //sudan baþka bir sývý eklenecekse metot deðiþmeli
        if (mainLiquidMesh.gameObject.activeSelf) {
            mainLiquidMesh.gameObject.SetActive(false);
        }
    }
}
