using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class DensityTowerVisualizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] layers;
    [SerializeField] private float animationDuration = 1.2f;

    /// <summary>
    /// Geriye dönük uyumluluk için (Eski çağrılar bozulmasın diye)
    /// </summary>
    public void UpdateDensityVisuals(List<LabObjectSO> liquidData)
    {
        UpdateDensityVisuals(liquidData, animationDuration);
    }

    /// <summary>
    /// Dökülme süresiyle paralel yükselme animasyonunu yöneten ana metod.
    /// </summary>
    public void UpdateDensityVisuals(List<LabObjectSO> liquidData, float customDuration)
    {
        if (layers == null || layers.Length == 0) return;

        // Yoğunluk sıralaması
        var sorted = liquidData.OrderByDescending(x => x.density).ToList();

        for (int i = 0; i < layers.Length; i++)
        {
            MeshRenderer currentLayer = layers[i];

            // KRİTİK: Sadece listedeki eleman sayısı kadar katmanı aktif et
            if (i < sorted.Count)
            {
                LabObjectSO data = sorted[i];
                float targetHeight = 0.35f;

                // Eğer katman henüz aktif değilse, animasyonla aç
                if (!currentLayer.gameObject.activeSelf)
                {
                    currentLayer.gameObject.SetActive(true);
                    currentLayer.transform.DOKill(); // Eski animasyonu temizle
                    currentLayer.transform.localScale = new Vector3(1, 0, 1);
                    currentLayer.material.color = data.color;

                    currentLayer.transform.DOScaleY(targetHeight, customDuration).SetEase(Ease.Linear);
                }
                else
                {
                    // Zaten aktif olan katmanların yerini ve rengini koru/güncelle
                    currentLayer.material.DOColor(data.color, 0.25f);
                    currentLayer.transform.localScale = new Vector3(1, targetHeight, 1);
                }
            }
            else
            {
                // Veri listesinde karşılığı olmayan katmanları kesinlikle kapat
                currentLayer.gameObject.SetActive(false);
            }
        }
    }
}