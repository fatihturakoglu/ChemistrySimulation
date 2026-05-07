using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MouseClickInventory : MonoBehaviour
{
    [Header("Referanslar")]
    public SCInventory playerInventory;
    public InventoryUI inventoryUI;
    public Camera mainCamera;

    [Header("Ayarlar")]
    public float etkilesimMesafesi = 3f;
    public KeyCode etkilesimTusu = KeyCode.E;

    [Header("UI Ayarları")]
    public TextMeshProUGUI etkilesimYazisi;
    public Vector3 yaziOffseti = new Vector3(0, 0.15f, 0);

    private Item hedeflenenDunyaEsyasi;
    private ItemPlace hedeflenenMasaSlotu;

    private void Update()
    {
        // F tuşu ile inceleme
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (hedeflenenDunyaEsyasi != null && hedeflenenDunyaEsyasi.item != null)
            {
                ItemInspectionManager.Instance.StartInspection(hedeflenenDunyaEsyasi.item);
            }
            else if (hedeflenenMasaSlotu != null && hedeflenenMasaSlotu.isOccupied)
            {
                ItemInspectionManager.Instance.StartInspection(hedeflenenMasaSlotu.placedItem);
            }
        }

        // Etkileşim tespiti
        EtkilesimKontrolu();

        // E tuşu ile etkileşim
        if (Input.GetKeyDown(etkilesimTusu))
        {
            if (hedeflenenDunyaEsyasi != null)
            {
                EsyayiAl();
            }
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
            Item worldItem = hit.collider.GetComponent<Item>();
            ItemPlace chemSlot = hit.collider.GetComponent<ItemPlace>();

            // Çarptığımız obje silinmişse veya pasifse hedefi sıfırla
            if (hit.collider == null || !hit.collider.gameObject.activeInHierarchy)
            {
                Sifirla();
                return;
            }

            if (worldItem != null)
            {
                string esyaIsmi = worldItem.item != null ? worldItem.item.itemName : "Eşya";
                SetTarget(worldItem, null, hit.transform.position, " Almak için [" + etkilesimTusu + "] " + esyaIsmi);
            }
            else if (chemSlot != null)
            {
                // Fiziksel olarak görsel yoksa ama dolu görünüyorsa düzelt
                if (chemSlot.isOccupied && chemSlot.currentVisual == null)
                {
                    chemSlot.isOccupied = false;
                }

                if (!chemSlot.isOccupied)
                {
                    SetTarget(null, chemSlot, hit.transform.position, "Buraya Koy [" + etkilesimTusu + "]");
                }
                else
                {
                    string esyaIsmi = chemSlot.placedItem != null ? chemSlot.placedItem.itemName : "Eşya";
                    SetTarget(null, chemSlot, hit.transform.position, esyaIsmi + " Geri Al [" + etkilesimTusu + "]");
                }
            }
            else { Sifirla(); }
        }
        else { Sifirla(); }
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
            // Yazıyı anında kapat ki kopyalama tetiklenmesin
            etkilesimYazisi.gameObject.SetActive(false);

            hedeflenenDunyaEsyasi.transform.DOKill();
            Destroy(hedeflenenDunyaEsyasi.gameObject);
            inventoryUI.UpdateUI();
            Sifirla();
        }
    }

    void EsyayiMasayaKoy()
    {
        if (hedeflenenMasaSlotu.isOccupied)
        {
            if (playerInventory.AddItem(hedeflenenMasaSlotu.placedItem))
            {
                etkilesimYazisi.gameObject.SetActive(false);
                hedeflenenMasaSlotu.RemoveItem();
                inventoryUI.UpdateUI();
                Sifirla();
            }
        }
        else
        {
            int seciliIndex = inventoryUI.selectedSlotIndex;
            if (seciliIndex < playerInventory.InventorySlots.Count)
            {
                Slot envanterSlotu = playerInventory.InventorySlots[seciliIndex];
                if (envanterSlotu.isFull && envanterSlotu.item != null)
                {
                    hedeflenenMasaSlotu.PlaceItem(envanterSlotu.item);
                    envanterSlotu.item = null;
                    envanterSlotu.isFull = false;
                    inventoryUI.UpdateUI();
                    Sifirla();
                }
            }
        }
    }
}