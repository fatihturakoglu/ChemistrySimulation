using UnityEngine;

public class BookController : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject bookUIPrefab;
    public Transform canvasTransform;
    private GameObject _activeBookUI;

    [Header("Ayarlar")]
    public string bookTag = "ChemistryBook";

    void Update()
    {
        // Fare tıklaması kontrolü
        if (Input.GetMouseButtonDown(0))
        {
            HandleBookClick();
        }

        // ESC ile kitabı kapatma (Opsiyonel ama kullanışlı)
        if (Input.GetKeyDown(KeyCode.Escape) && _activeBookUI != null)
        {
            KitabiKapat();
        }
    }

    private void HandleBookClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(bookTag))
            {
                KitabiAc();
            }
        }
    }

    public void KitabiAc()
    {
        if (_activeBookUI == null)
        {
            _activeBookUI = Instantiate(bookUIPrefab, canvasTransform);

            // KRİTİK SATIR: Prefab kapalıysa bile burada açıyoruz
            _activeBookUI.SetActive(true);

            RectTransform rt = _activeBookUI.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localPosition = Vector3.zero;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Parametresiz, tertemiz kapatma fonksiyonu
    public void KitabiKapat()
    {
        if (_activeBookUI != null)
        {
            Destroy(_activeBookUI); // Obje tamamen silinsin
            _activeBookUI = null;   // DEĞİŞKENİ BOŞALTIYORUZ (Tekrar açılabilmesi için kritik!)

            // İmleci tekrar kilitlemek istersen (FPS karakterin varsa)
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }
    }
}