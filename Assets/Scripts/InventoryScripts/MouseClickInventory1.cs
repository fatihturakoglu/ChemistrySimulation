using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MouseClickInventory : MonoBehaviour
{
    [Header("Referanslar")]
    public SCInventory playerInventory;
    public InventoryUI inventoryUI; // Seçili slotu öğrenmek için gerekli
    public Camera mainCamera;

    [Header("Ayarlar")]
    public float etkilesimMesafesi = 3f;
    public KeyCode etkilesimTusu = KeyCode.E;

    [Header("UI Ayarları")]
    public TextMeshProUGUI etkilesimYazisi;
    public Vector3 yaziOffseti = new Vector3(0, 0.5f, 0);

    private Item hedeflenenDunyaEsyasi; // Yerden alınacak eşya
    private ItemPlace hedeflenenMasaSlotu; // Masadaki koyulacak yer

    private void Update()
    {
        EtkilesimKontrolu();

        if (Input.GetKeyDown(etkilesimTusu))
        {
            // Eğer bir eşyaya bakıyorsak AL
            if (hedeflenenDunyaEsyasi != null)
            {
                EsyayiAl();
            }
            // Eğer masadaki bir boşluğa bakıyorsak KOY
            else if (hedeflenenMasaSlotu != null)
            {
                EsyayiMasayaKoy();
            }
        }
    }

    void EtkilesimKontrolu()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, etkilesimMesafesi))
        {
            // 1. DURUM: Yerdeki bir eşyaya mı bakıyoruz?
            Item worldItem = hit.collider.GetComponent<Item>();
            // 2. DURUM: Masadaki bir slota mı bakıyoruz?
            ItemPlace chemSlot = hit.collider.GetComponent<ItemPlace>();

            if (worldItem != null)
            {
                SetTarget(worldItem, null, hit.transform.position, "Almak için [" + etkilesimTusu + "]");
            }
            else if (chemSlot != null)
            {
                if (!chemSlot.isOccupied)
                {
                    SetTarget(null, chemSlot, hit.transform.position, "Koymak için [" + etkilesimTusu + "]");
                }
                else
                {
                    SetTarget(null, chemSlot, hit.transform.position, "Eşyayı Geri Al [" + etkilesimTusu + "]");
                }
            }
            else
            {
                Sifirla();
            }
        }
        else
        {
            Sifirla();
        }
    }

    void SetTarget(Item item, ItemPlace slot, Vector3 pos, string txt)
    {
        hedeflenenDunyaEsyasi = item;
        hedeflenenMasaSlotu = slot;

        etkilesimYazisi.gameObject.SetActive(true);
        etkilesimYazisi.text = txt;

        Vector3 ekranPozisyonu = mainCamera.WorldToScreenPoint(pos + yaziOffseti);
        etkilesimYazisi.transform.position = ekranPozisyonu;
    }

    void Sifirla()
    {
        hedeflenenDunyaEsyasi = null;
        hedeflenenMasaSlotu = null;
        if (etkilesimYazisi != null) etkilesimYazisi.gameObject.SetActive(false);
    }

    void EsyayiAl()
    {
        if (playerInventory.AddItem(hedeflenenDunyaEsyasi.item))
        {
            hedeflenenDunyaEsyasi.transform.DOKill();
            Destroy(hedeflenenDunyaEsyasi.gameObject);
            inventoryUI.UpdateUI(); // UI'ı tazele
            Sifirla();
        }
        else
        {
            Debug.Log("Çanta dolu!");
        }
    }

    void EsyayiMasayaKoy()
    {
        // Masadaki slot doluysa geri al, boşsa envanterden koy
        if (hedeflenenMasaSlotu.isOccupied)
        {
            if (playerInventory.AddItem(hedeflenenMasaSlotu.placedItem))
            {
                hedeflenenMasaSlotu.RemoveItem();
                inventoryUI.UpdateUI();
                Sifirla();
            }
        }
        else
        {
            // Envanterde seçili olan slotu bul
            int seciliIndex = inventoryUI.selectedSlotIndex;

            if (seciliIndex < playerInventory.InventorySlots.Count)
            {
                Slot envanterSlotu = playerInventory.InventorySlots[seciliIndex];

                if (envanterSlotu.isFull && envanterSlotu.item != null)
                {
                    // Masaya yerleştir
                    hedeflenenMasaSlotu.PlaceItem(envanterSlotu.item);

                    // Envanterden temizle
                    envanterSlotu.item = null;
                    envanterSlotu.isFull = false;

                    inventoryUI.UpdateUI(); // Görseli güncelle
                    Sifirla();
                }
                else
                {
                    Debug.Log("Seçili slot boş! Masaya bir şey koyamazsın.");
                }
            }
        }
    }
}