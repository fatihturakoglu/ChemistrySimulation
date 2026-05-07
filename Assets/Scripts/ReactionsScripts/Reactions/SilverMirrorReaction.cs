using UnityEngine;
using VContainer;
using DG.Tweening;
using System.Linq;

public class SilverMirrorReaction : MonoBehaviour
{
    [SerializeField] private Material silverMirrorMaterial;
    private IBeaker _beaker;
    private float silverFormationDuration = 6f; // Ayna oluşma hızı

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        if (_beaker == null) return;

        var beakerObj = (_beaker as MonoBehaviour).gameObject;
        var visualizer = beakerObj.GetComponent<SilverMirrorVisualizer>();

        if (visualizer != null)
        {
            // 1. Sıvıyı (suyu) tamamen kapat
            if (visualizer.liquidRenderer != null)
                visualizer.liquidRenderer.gameObject.SetActive(false);

            // 2. Beherin camını senin orijinal materyalinle kapla
            if (visualizer.glassRenderer != null)
            {
                visualizer.glassRenderer.sharedMaterial = silverMirrorMaterial;
            }

            // 3. İçteki boru (silverMirror) objesini bul ve canlandır
            // Tüm çocukları tarayıp isme göre buluyoruz (En garantisi)
            Transform internalMirror = beakerObj.GetComponentsInChildren<Transform>(true)
                                      .FirstOrDefault(t => t.name == "silverMirror");

            if (internalMirror != null)
            {
                internalMirror.gameObject.SetActive(true);
                MeshRenderer mirrorRenderer = internalMirror.GetComponent<MeshRenderer>();

                // Beyazlatma yapmadan direkt materyali veriyoruz
                mirrorRenderer.sharedMaterial = silverMirrorMaterial;

                // Animasyon: Aşağıdan yukarıya doğru gümüş kaplanıyor (ScaleY)
                internalMirror.localScale = new Vector3(internalMirror.localScale.x, 0, internalMirror.localScale.z);
                internalMirror.DOScaleY(0.13f, silverFormationDuration).SetEase(Ease.OutQuad);

                Debug.Log("<color=silver>GÜMÜŞ AYNA:</color> Orijinal materyal ile animasyon başladı.");
            }
        }
    }
}