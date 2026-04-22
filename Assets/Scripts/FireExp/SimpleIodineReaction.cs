using UnityEngine;

public class SimpleIodineReaction : MonoBehaviour
{
    [Header("Ayarlar")]
    public string targetObjectName = "Kat??yot"; // Takip edilecek objenin ad?
    public GameObject purpleSmokePrefab;       // Ç?kacak mor duman prefab?
    public Transform spawnPoint;              // Duman?n ç?kaca?? konum
    public float dissolveSpeed = 0.5f;        // Eriyme h?z?

    private GameObject activeIodine;
    private bool isDissolving = false;

    void Update()
    {
        // 1. E?er sahnede iyot yoksa, ismine göre bul
        if (activeIodine == null)
        {
            activeIodine = GameObject.Find(targetObjectName);
        }

        // 2. E?er iyot bulunduysa ve ocak yan?yorsa (SpiritLamp'ten kontrol al?yoruz)
        if (activeIodine != null && SpiritLamp.Instance != null && SpiritLamp.Instance.IsLit())
        {
            StartDissolving();
        }
    }

    void StartDissolving()
    {
        if (isDissolving) return;
        isDissolving = true;
        StartCoroutine(DissolveRoutine());
    }

    System.Collections.IEnumerator DissolveRoutine()
    {
        Vector3 startScale = activeIodine.transform.localScale;
        float progress = 0;

        while (progress < 1f)
        {
            // E?er oyuncu oca?? kapat?rsa erime dursun
            if (!SpiritLamp.Instance.IsLit())
            {
                isDissolving = false;
                yield break;
            }

            progress += Time.deltaTime * dissolveSpeed;
            activeIodine.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            yield return null;
        }

        // ?yot tamamen silindi?inde
        Destroy(activeIodine);

        // Mor duman? ç?kart
        if (purpleSmokePrefab != null)
        {
            // Senin seçti?in konumda (spawnPoint) olu?tur
            GameObject smoke = Instantiate(purpleSmokePrefab, spawnPoint.position, Quaternion.identity);

            // 5 saniye sonra duman? sahneden temizle
            Destroy(smoke, 5f);
        }

        isDissolving = false;
        activeIodine = null;
    }
}