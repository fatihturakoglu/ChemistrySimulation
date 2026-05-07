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

        if (item.itemPrefab != null)
        {
            currentVisual = Instantiate(item.itemPrefab, spawnPoint.position, spawnPoint.rotation);
            currentVisual.transform.SetParent(this.transform);

            // --- ZEMİNE OTURTMA MANTIĞI ---
            SnapToGround(currentVisual);
        }
    }

    private void SnapToGround(GameObject obj)
    {
        // Objede Renderer (görsel) var mı bak
        MeshRenderer renderer = obj.GetComponentInChildren<MeshRenderer>();
        if (renderer == null) return;

        // Objenin en alt noktasının dünya koordinatındaki yerini bul
        float bottomY = renderer.bounds.min.y;

        // Spawn noktasının Y koordinatı ile objenin en altı arasındaki farkı bul
        float offset = spawnPoint.position.y - bottomY;

        // Objeyi bu fark kadar yukarı/aşağı kaydır
        obj.transform.position += new Vector3(0, offset, 0);
    }

    public void RemoveItem()
    {
        if (currentVisual != null)
        {
            // GÜVENLİK: Objeyi Destroy etmeden önce anında pasif yapıyoruz.
            // Böylece Raycast "E"ye bastığın mikro saniyede bu objeye çarpamaz.
            currentVisual.SetActive(false);
            Destroy(currentVisual);
            currentVisual = null;
        }

        placedItem = null;
        isOccupied = false;
    }
}
