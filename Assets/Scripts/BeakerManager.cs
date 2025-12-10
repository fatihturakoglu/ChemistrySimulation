using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;

    private List<LabObject> labObjects;
    private List<string> labObjectNames;

    private void Start()
    {
        ingredientManager.OnIngredientAdded += ÝngredientManager_OnIngredientAdded;
        labObjects = new List<LabObject>();
        labObjectNames = new List<string>(); //LÝSTELERÝ SIFIRLAMAZSAN ÇÖKÜYOR
                                             //niye oluyor geminiye sor
    }

    private void ÝngredientManager_OnIngredientAdded(object sender, IngredientManager.OnIngredientAddedEventArgs e) {
        //bu event her tetiklendiðinde tarifleri for döngüsü ile kontrol edeceðiz
        labObjects.Add(e.labObject);
        Debug.Log(e.labObject.GetLabObjectSO().objectName);
        CheckRecipes();
    }

    private void Update()
    {
        
    }

    private void CheckRecipes() {
        labObjectNames.Clear();
        foreach(LabObject ingredient in labObjects) {
            labObjectNames.Add(ingredient.GetLabObjectSO().objectName);
        }

        if(labObjectNames.Contains("Sodium") && labObjectNames.Contains("Water")) {

            Debug.Log("BUM");
            foreach (LabObject ingredient in labObjects) {
                if(!ingredient.GetLabObjectSO().isReusable)
                Destroy(ingredient.gameObject); //tekrar kullanýlmayan maddeler yok olsun
            }

            labObjects.Clear(); //tepkime olunca beherdeki tüm maddeler sýfýrlansýn
            
        }
    }
}
