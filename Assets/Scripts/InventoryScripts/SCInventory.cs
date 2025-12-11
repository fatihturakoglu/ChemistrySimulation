using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "ScriptableObjects/Inventory")]
public class SCInventory : ScriptableObject
{
    public List<Slot> InventorySlots = new List<Slot>();

    public bool AddItem(SCItem newItem)
    {
        foreach (Slot slot in InventorySlots)
        {
            if (slot.isFull == false) 
            {
                slot.addItemToSlot(newItem);
                slot.isFull = true;
                return true; 
            }
        }

        Debug.Log("Çanta Tamamen Dolu! Eşya alınamadı.");
        return false; 
    }
}

[System.Serializable]
public class Slot
{
    public bool isFull;
    public SCItem item;

    public void addItemToSlot(SCItem item)
    {
        this.item = item;
    }
}