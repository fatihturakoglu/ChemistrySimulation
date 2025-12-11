using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Beaker : MonoBehaviour {
    [SerializeField] private IngredientManager ingredientManager;

    [SerializeField] private List<RecipeSO> allRecipes;

    private List<LabObject> labObjects;

    private void Start() {
        ingredientManager.OnIngredientAdded += ÝngredientManager_OnIngredientAdded;
        labObjects = new List<LabObject>();
    }

    private void ÝngredientManager_OnIngredientAdded(object sender, IngredientManager.OnIngredientAddedEventArgs e) {
        AddIngredient(e.labObject);
        Debug.Log(e.labObject.GetLabObjectSO().objectName + " þuan beherde mevcut.");
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
                PerformReaction(recipe);
                return; // Stop checking after finding a match
            }
        }
    }

    private void PerformReaction(RecipeSO recipe) {
        Debug.Log("Tepkime baþladý: " + recipe.recipeName);
        float yOffset = 0.1f;
        Vector3 reactionPos = transform.position;
        reactionPos.y += yOffset;

        if (recipe.reaction != null) {
            Instantiate(recipe.reaction, reactionPos, Quaternion.identity);
        }

        ResetBeaker();
    }

    private void ResetBeaker() {
        foreach (LabObject ingredient in labObjects) {
            if (!ingredient.GetLabObjectSO().isReusable)
                Destroy(ingredient.gameObject); //tekrar kullanýlmayan maddeler yok olsun
        }

        labObjects.Clear(); //tepkime olunca beherdeki tüm maddeler sýfýrlansýn
    }

    private void AddIngredient(LabObject labObject) {
        labObjects.Add(labObject);
        CheckRecipes();
    }
}
