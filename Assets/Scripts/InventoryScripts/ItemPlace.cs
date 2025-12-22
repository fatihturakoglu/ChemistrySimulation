using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlace : MonoBehaviour
{
    public bool isOccupied = false; // Dolu mu?
    public SCItem placedItem;       // İçindeki eşya verisi
    public GameObject currentVisual; // Masada duran 3D model
    public Transform spawnPoint;    // Eşyanın tam nereye konulacağı

    public void PlaceItem(SCItem item)
    {
        placedItem = item;
        isOccupied = true;

        // Eşyanın 3D modelini masada oluştur
        if (item.itemPrefab != null)
        {
            currentVisual = Instantiate(item.itemPrefab, spawnPoint.position, spawnPoint.rotation);
            currentVisual.transform.SetParent(this.transform);
        }
    }

    public void RemoveItem()
    {
        Destroy(currentVisual);
        placedItem = null;
        isOccupied = false;
    }
}
