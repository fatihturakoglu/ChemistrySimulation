using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening; // DOTween kütüphanesini eklemeyi unutma

public class DensityTowerVisualizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] layers; // Element 0 en alt olmalı
    [SerializeField] private float animationDuration = 1.2f; // Yer değiştirme hızı

    public void UpdateDensityVisuals(List<LabObjectSO> liquidData)
    {
        // Önce sahnede olmayan üst katmanları kapat
        for (int i = liquidData.Count; i < layers.Length; i++)
        {
            layers[i].gameObject.SetActive(false);
        }

        // Yoğunluğa göre sırala (En yoğun en alta)
        var sorted = liquidData.OrderByDescending(x => x.density).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            if (i < layers.Length)
            {
                MeshRenderer currentLayer = layers[i];
                Color targetColor = sorted[i].color;

                // Eğer katman kapalıysa (ilk kez ekleniyorsa)
                if (!currentLayer.gameObject.activeSelf)
                {
                    currentLayer.gameObject.SetActive(true);
                    // Başlangıçta şeffaf veya beyazdan başlasın
                    currentLayer.material.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0);
                    currentLayer.material.DOColor(targetColor, animationDuration);
                }
                else
                {
                    // Katman zaten açıksa (yer değiştirme oluyorsa)
                    // DOTween ile mevcut renkten hedef renge yumuşak geçiş yap
                    currentLayer.material.DOColor(targetColor, animationDuration)
                        .SetEase(Ease.InOutQuad); // Daha doğal bir süzülme hissi
                }

                // Dolgun görünüm için scale ayarı
                currentLayer.transform.localScale = new Vector3(1, 0.35f, 1);
            }
        }
    }
}