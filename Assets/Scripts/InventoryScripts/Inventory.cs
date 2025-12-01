using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SCInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item worldItem = other.GetComponent<Item>();

            if (worldItem != null && worldItem.item != null)
            {
                bool eklendi = playerInventory.AddItem(worldItem.item);

                if (eklendi)
                {
                    Destroy(other.gameObject); // Sadece çantaya girdiyse yok et
                }
                else
                {
                    Debug.Log("Çanta dolu, eşya alınamadı!");
                }
            }
        }
    }
}