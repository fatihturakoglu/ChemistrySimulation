using UnityEngine;
using UnityEngine.UI;

public class CloseBookHelper : MonoBehaviour
{
    void Start()
    {
        // Butona tıklandığında sahnede BookController olan objeyi bulur
        GetComponent<Button>().onClick.AddListener(() => {
            BookController bc = FindObjectOfType<BookController>();
            if (bc != null)
            {
                bc.KitabiKapat();
            }
        });
    }
}