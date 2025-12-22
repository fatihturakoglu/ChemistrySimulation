using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public SCInventory inventoryData;

    [Header("Seçim Ayarları")]
    public int selectedSlotIndex = 0;

    InventorySlot[] uiSlots;

    void Start()
    {
        uiSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
        UpdateSlotVisuals();
    }

    void Update()
    {
        // 1. Sayı Tuşları (Düzeltildi: Her biri farklı KeyCode ve Index)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SetSelectedSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SetSelectedSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SetSelectedSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SetSelectedSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SetSelectedSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SetSelectedSlot(5); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SetSelectedSlot(6); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SetSelectedSlot(7); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { SetSelectedSlot(8); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { SetSelectedSlot(9); }

        // 2. Mouse Tekerleği
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            int nextSlot = selectedSlotIndex - 1;
            if (nextSlot < 0) nextSlot = uiSlots.Length - 1;
            SetSelectedSlot(nextSlot);
        }
        else if (scroll < 0f)
        {
            int nextSlot = selectedSlotIndex + 1;
            if (nextSlot >= uiSlots.Length) nextSlot = 0;
            SetSelectedSlot(nextSlot);
        }

        // DİKKAT: Buradaki UpdateUI() silindi! Sadece veri değişince MouseClickInventory üzerinden çağrılacak.
    }

    void SetSelectedSlot(int index)
    {
        if (uiSlots != null && index >= 0 && index < uiSlots.Length)
        {
            selectedSlotIndex = index;
            UpdateSlotVisuals();
            Debug.Log("Seçili Slot: " + selectedSlotIndex);
        }
    }

    void UpdateSlotVisuals()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            // selectionFrame'i açıp kapatır
            uiSlots[i].SetSelected(i == selectedSlotIndex);
        }
    }

    public void UpdateUI()
    {
        if (uiSlots == null) return;

        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i < inventoryData.InventorySlots.Count)
            {
                Slot dataSlot = inventoryData.InventorySlots[i];
                if (dataSlot.isFull && dataSlot.item != null)
                {
                    uiSlots[i].AddItem(dataSlot.item);
                }
                else
                {
                    uiSlots[i].ClearSlot();
                }
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
    }
}