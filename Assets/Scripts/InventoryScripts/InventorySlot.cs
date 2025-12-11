using UnityEngine;
using UnityEngine.UI; // UI işlemleri için şart
using TMPro;    // TextMeshPro işlemleri için şart

public class InventorySlot : MonoBehaviour
{
    public Image icon;          // İçindeki "Icon" objesi
    public TMP_Text itemName; 

    SCItem item;  // Bu slotta şu an hangi eşya verisi var?

    // Slota eşya ekleme fonksiyonu
    public void AddItem(SCItem newItem)
    {
        item = newItem;
        itemName.text = item.itemName; 
        icon.sprite = item.itemIcon; 
        icon.enabled = true;         
    }

    public void ClearSlot()
    {
        item = null;
        if (itemName != null) itemName.text = ""; // Yazıyı temizle

        icon.sprite = null; // Resmi kaldır
        icon.enabled = false; // Resmi gizle
    }
    public void OnUseButton()
    {
        if (item != null)
        {
            Debug.Log(item.itemName + " kullanıldı!");
        }
    }
}