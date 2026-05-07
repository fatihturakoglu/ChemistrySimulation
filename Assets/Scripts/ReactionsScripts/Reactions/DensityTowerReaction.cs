using UnityEngine;
using VContainer;

public class DensityTowerReaction : MonoBehaviour
{
    [SerializeField] private LabObjectSO[] liquids;
    private IBeaker _beaker;
    private float life = 10f; // Sadece mantıksal tetikleme için kısa bir süre yeterli

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        Debug.Log("Yoğunluk Kulesi Tamamlandı!");

        // Bu script artık sadece görsel bir "başarı" efekti (belki konfeti veya ses) 
        // tetiklemek için kullanılmalı. Sıvıların kontrolü zaten BeakerManager'da.

        Destroy(gameObject, life);
    }

    // OnDestroy metodunu sildik çünkü katmanların kapanmasını istemiyoruz!
}