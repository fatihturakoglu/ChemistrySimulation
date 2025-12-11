using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;   // Slotların olduğu Panel
    public SCInventory inventoryData; // ScriptableObject verisi

    InventorySlot[] uiSlots; // Ekrandaki slot kutucukları

    void Start()
    {
        // Panel'in altındaki tüm slot scriptlerini bul
        uiSlots = itemsParent.GetComponentsInChildren<InventorySlot>();

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        // UI Slotlarımız kadar dönüyoruz
        for (int i = 0; i < uiSlots.Length; i++)
        {
            // Eğer veri tabanında bu sırada bir slot tanımı varsa
            if (i < inventoryData.InventorySlots.Count)
            {
                // Veri tabanındaki o slotu al (isFull ve item bilgisini tutan class)
                Slot dataSlot = inventoryData.InventorySlots[i];

                // Eğer o slot doluysa ve içinde item varsa
                if (dataSlot.isFull == true && dataSlot.item != null)
                {
                    uiSlots[i].AddItem(dataSlot.item); // UI'ı güncelle
                }
                else
                {
                    uiSlots[i].ClearSlot(); // Boşalt
                }
            }
            else
            {
                uiSlots[i].ClearSlot(); // Veri yoksa boşalt
            }
        }
    }
}