using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]

public class SCItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject itemPrefab;
}
