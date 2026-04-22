using UnityEngine;

public class SimpleEffectTrigger : MonoBehaviour
{
    [Header("Takip Edilecek Obje")]
    public GameObject targetObject; // Sahnede silinecek olan iyot

    [Header("Efekt Ayarları")]
    public GameObject effectPrefab; // Çıkacak mor duman
    public Transform spawnLocation; // Dumanın çıkacağı yer

    private bool _hasTriggered = false;

    void Update()
    {
        // Eğer hedef obje sahnede yoksa (null olduysa) ve daha önce tetiklenmediyse
        if (targetObject == null && !_hasTriggered)
        {
            SpawnEffect();
            _hasTriggered = true;
        }
    }

    void SpawnEffect()
    {
        if (effectPrefab != null)
        {
            // Belirlediğin konumda dumanı oluştur
            GameObject fx = Instantiate(effectPrefab, spawnLocation.position, spawnLocation.rotation);

            // 7 saniye sonra dumanı sahnede kalabalık yapmasın diye sil
            Destroy(fx, 7f);

            Debug.Log("İyot yok oldu, mor duman çıktı!");
        }
    }
}