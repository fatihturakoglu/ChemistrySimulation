using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public GameObject selectionFrame;

    private SCItem item;

    public void AddItem(SCItem newItem)
    {
        // Eğer zaten aynı eşya varsa, atama yapma ki animasyon bozulmasın
        if (item == newItem) return;

        item = newItem;
        if (itemName != null) itemName.text = item.itemName;

        icon.sprite = item.itemIcon;
        icon.enabled = true;

        // Rotasyonu her eklemede sıfırla (Bozulmayı önler)
        icon.rectTransform.localRotation = Quaternion.identity;
    }

    public void ClearSlot()
    {
        item = null;
        if (itemName != null) itemName.text = "";
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetSelected(bool state)
    {
        if (selectionFrame != null)
            selectionFrame.SetActive(state);
    }
}