using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<LabObjectSO> requiredIngredients;
    public GameObject reaction; //fonksiyon atasak daha iyi olur?
}
