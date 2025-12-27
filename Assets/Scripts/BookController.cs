using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public GameObject kitapPaneli; // Canvas içindeki Panel

    void Update()
    {
        // Fare sol tık basıldığında
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Objeyi ismiyle değil, "Kitap" etiketiyle kontrol ediyoruz
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
        // Fareyi serbest bırakmak (UI etkileşimi için önemli olabilir)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void KitabiKapat()
    {
        kitapPaneli.SetActive(false);
        // FPS kontrolü varsa burada fareyi tekrar kilitleyebilirsiniz
    }
}