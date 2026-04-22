using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI; // Tween kütüphanesi yüklü olduğu için kullanalım

public class ItemInspectionManager : MonoBehaviour
{
    public static ItemInspectionManager Instance;

    [Header("Referanslar")]
    public Transform inspectionAnchor; // Kameranın içindeki boş obje item burada'Ki konumda oluşur 
    public GameObject inspectionUIPanel; // Siyah karartma paneli
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public GameObject Button_Close; // Kapatma butonu
    public GameObject itemImage;

    private GameObject currentClone;
    private bool isInspecting = false;
    private float rotationSpeed = 200f;

    private void Awake() => Instance = this;

    public void StartInspection(SCItem item)
    {
        if (isInspecting) return;

        isInspecting = true;
        inspectionUIPanel.SetActive(true);
        Button_Close.gameObject.SetActive(true);

        // Eşyanın prefabını kameranın önündeki noktada oluştur
        currentClone = Instantiate(item.itemPrefab, inspectionAnchor.position, inspectionAnchor.rotation);

        // Eşyayı kameranın "çocuğu" yap (Kamera dönerse eşya da onunla kalsın)
        currentClone.transform.SetParent(inspectionAnchor);

        // Eşyayı hafifçe büyütmek veya öne getirmek için küçük bir animasyon
        currentClone.transform.localScale = Vector3.one * 1.5f;

        itemNameText.text = item.itemName;
        itemDescText.text = item.detailedInfo; // Ya da detaylı bilgi
    }

    private void Update()
    {
        if (!isInspecting) return;

        // Mouse ile döndürme
        if (Input.GetMouseButton(0))
        {
            float h = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            currentClone.transform.Rotate(Vector3.up, -h, Space.World);
            currentClone.transform.Rotate(Vector3.right, v, Space.World);
        }

        // Kapatma
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
        {
            StopInspection();
        }
    }

    public void StopInspection()
    {
        isInspecting = false;
        inspectionUIPanel.SetActive(false);
        if (currentClone != null) Destroy(currentClone);
        Button_Close.gameObject.SetActive(false);
    }
}