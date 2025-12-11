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
            Item worldItem = other.GetComponent<Item>(); // Çarptığımız objenin üzerindeki Item scriptine ulaşmaya çalışır.(other çarptığımız objeyi temsil eder)

            if (worldItem != null && worldItem.item != null)
            {
                bool eklendi = playerInventory.AddItem(worldItem.item);   // add item methodundan true ya da false döner ona göre ekleme işlemi gerçekleşir

                if (eklendi)
                {
                    Destroy(other.gameObject); // Sadece çantaya girdiyse yok et
                    Debug.Log("Eşya alındı: " + worldItem.item.itemName);
                }
                else
                {
                    Debug.Log("Çanta dolu, eşya alınamadı!");
                }
            }
        }
    }



}