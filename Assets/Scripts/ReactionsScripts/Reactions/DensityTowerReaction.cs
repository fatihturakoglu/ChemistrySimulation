using UnityEngine;
using VContainer;

public class DensityTowerReaction : MonoBehaviour
{
    [SerializeField] private LabObjectSO[] liquids;
    private IBeaker _beaker;
    private float life = 3f;

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        Debug.Log("Yoğunluk Kulesi Tepkimesi");

        // Ana sıvıyı kapat
        _beaker.MainLiquid.gameObject.SetActive(false);

        // Katmanlı sıvıları aç ve renklerini ata
        var layeredLiquids = _beaker.LayeredLiquids;
        for (int i = 0; i < layeredLiquids.Length; i++)
        {
            if (i < liquids.Length) // Hata almamak için kontrol
            {
                layeredLiquids[i].gameObject.SetActive(true);
                layeredLiquids[i].material.color = liquids[i].color;
            }
        }

        Destroy(gameObject, life);
    }

    private void OnDestroy()
    {
        // Tepkime bitince katmanları geri kapat
        foreach (var layer in _beaker.LayeredLiquids)
        {
            layer.gameObject.SetActive(false);
        }
    }
}