using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public GameObject kitapPaneli; // Ana Panel
    public GameObject[] sayfalar;  // Hazırladığın sayfa Image'larını buraya sürükle
    private int aktifSayfaIndex = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ChemistryBook"))
                {
                    KitabiAc();
                }
            }
        }
    }

    public void KitabiAc()
    {
        kitapPaneli.SetActive(true);
        aktifSayfaIndex = 0; // Kitap her açıldığında ilk sayfadan başlar
        SayfalariGuncelle();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SonrakiSayfa()
    {
        if (aktifSayfaIndex < sayfalar.Length - 1)
        {
            aktifSayfaIndex++;
            SayfalariGuncelle();
        }
    }

    public void OncekiSayfa()
    {
        if (aktifSayfaIndex > 0)
        {
            aktifSayfaIndex--;
            SayfalariGuncelle();
        }
    }

    private void SayfalariGuncelle()
    {
        // Tüm sayfaları kapat, sadece aktif olanı aç
        for (int i = 0; i < sayfalar.Length; i++)
        {
            sayfalar[i].SetActive(i == aktifSayfaIndex);
        }
    }

    public void KitabiKapat()
    {
        kitapPaneli.SetActive(false);
    }
}