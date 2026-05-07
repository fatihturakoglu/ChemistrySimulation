using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<LabObjectSO> requiredIngredients;
    public GameObject reaction;

    [Header("Sıralama ve Miktar Kontrolü")]
    public bool requiresSpecificOrder; // Yeni: Sıralama önemli mi?
    public bool requiresSpecificDropCount;
    public string dropTargetName;
    public int requiredDropCount;

}