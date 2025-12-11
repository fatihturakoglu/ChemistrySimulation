using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro kullanmak için gerekli kütüphane
using DG.Tweening; // DOTween hatasını önlemek için

public class MouseClickInventory : MonoBehaviour
{
    [Header("Ayarlar")]
    public SCInventory playerInventory;
    public Camera mainCamera;
    public float almaMesafesi = 3f;
    public KeyCode almaTusu = KeyCode.E;

    [Header("UI Ayarları")]
    public TextMeshProUGUI etkilesimYazisi; // Hazırladığın Text objesini buraya sürükle
    public Vector3 yaziOffseti = new Vector3(0, 0.5f, 0); // Yazı eşyanın ne kadar üstünde dursun?

    private Item hedeflenenEsya; // Şu an baktığımız eşya

    private void Update()
    {
        EtkilesimKontrolu(); // Sürekli bakılan yeri kontrol et

        
        if (hedeflenenEsya != null && Input.GetKeyDown(almaTusu))
        {
            EsyayiAl();
        }
    }

    void EtkilesimKontrolu()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Işın bir şeye çarptı mı?
        if (Physics.Raycast(ray, out hit, almaMesafesi))
        {
            // Çarptığı şeyin üzerinde Item scripti var mı?
            Item worldItem = hit.collider.GetComponent<Item>();

            if (worldItem != null)
            {
                // EVET, BİR EŞYAYA BAKIYORUZ
                hedeflenenEsya = worldItem;

                // 1. Yazıyı görünür yap
                etkilesimYazisi.gameObject.SetActive(true);

                // 2. Yazıyı eşyanın 3D pozisyonundan 2D ekran pozisyonuna taşı
                Vector3 dunyaPozisyonu = hit.transform.position + yaziOffseti;
                Vector3 ekranPozisyonu = mainCamera.WorldToScreenPoint(dunyaPozisyonu);

                etkilesimYazisi.transform.position = ekranPozisyonu;

                // (İsteğe bağlı) Yazı içeriğini dinamik yapabilirsin
                // etkilesimYazisi.text = worldItem.item.itemName + " Almak için [E]";
            }
            else
            {
                // Eşya değil, başka bir duvara bakıyoruz
                Sifirla();
            }
        }
        else
        {
            // Hiçbir şeye bakmıyoruz (Boşluk)
            Sifirla();
        }
    }

    void Sifirla()
    {
        hedeflenenEsya = null;
        if (etkilesimYazisi != null) etkilesimYazisi.gameObject.SetActive(false);
    }

    void EsyayiAl()
    {
        if (hedeflenenEsya != null && hedeflenenEsya.item != null)
        {
            bool eklendi = playerInventory.AddItem(hedeflenenEsya.item);

            if (eklendi)
            {
                // DOTween hatasını önlemek için animasyonları öldür
                hedeflenenEsya.transform.DOKill();

                Destroy(hedeflenenEsya.gameObject);

                // Eşyayı aldık, artık bakılacak bir şey kalmadı, yazıyı gizle
                Sifirla();
            }
            else
            {
                Debug.Log("Çanta dolu!");
            }
        }
    }
}