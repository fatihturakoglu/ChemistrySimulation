using DG.Tweening;
using UnityEngine;

public class SilverMirrorVisualizer : MonoBehaviour
{
    [Header("Referanslar")]
    public MeshRenderer liquidRenderer;
    public MeshRenderer glassRenderer;
    public GameObject brownPrecipitate;

    [Header("Materyaller")]
    public Material silverMirrorMaterial;

    public void ShowBrownCloud()
    {
        if (brownPrecipitate != null) brownPrecipitate.SetActive(true);
    }

    public void ClearCloud()
    {
        if (brownPrecipitate != null) brownPrecipitate.SetActive(false);
    }

    public void PlayMirrorEffect()
    {
        if (glassRenderer == null) return;

        // sharedMaterial kullanarak senin orijinal materyal ayarlarını koruyoruz
        glassRenderer.sharedMaterial = silverMirrorMaterial;

        if (liquidRenderer != null)
            liquidRenderer.gameObject.SetActive(false);
    }

    public void PlayParallelRise(LabObjectSO so, float duration)
    {
        if (liquidRenderer == null) return;

        // Eklenen sıvının etkilediği yükseklik miktarını hesapla
        float addedAmount = so.colorIntensity * 0.62f;
        float currentScaleY = liquidRenderer.transform.localScale.y;

        // Maksimum doluluk sınırı (Örn: 1.5f)
        float targetHeight = Mathf.Min(currentScaleY + addedAmount, 1.5f);

        // Yükselme animasyonunu dökülme süresiyle (duration) eşitle
        liquidRenderer.transform.DOKill(); // Çakışmaları önlemek için eskiyi durdur
        liquidRenderer.transform.DOScaleY(targetHeight, duration).SetEase(Ease.Linear);
    }
}