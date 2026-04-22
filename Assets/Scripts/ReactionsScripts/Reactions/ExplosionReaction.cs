using UnityEngine;
using VContainer;

public class ExplosionReaction : MonoBehaviour
{
    private float life = 3f;
    private float destroyBeakerCounter = 2f;
    private MeshRenderer _beakerMesh;
    private IBeaker _beaker;

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        Debug.Log("Patlama Tepkimesi Başladı");
        Destroy(gameObject, life);

        if (_beaker is MonoBehaviour beakerMono)
        {
            _beakerMesh = beakerMono.GetComponentInChildren<MeshRenderer>();
        }

        DestroyBeaker();
    }

    private void Update()
    {
        destroyBeakerCounter -= Time.deltaTime;
        // Patlamadan 2 saniye sonra beheri geri getir
        if (destroyBeakerCounter <= 0 && _beakerMesh != null && !_beakerMesh.enabled)
            _beakerMesh.enabled = true;
    }

    private void DestroyBeaker()
    {
        if (_beakerMesh != null) _beakerMesh.enabled = false;
        _beaker.MainLiquid.gameObject.SetActive(false);
    }
}