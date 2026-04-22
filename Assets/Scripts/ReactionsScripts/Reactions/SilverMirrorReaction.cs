using UnityEngine;
using VContainer;

public class SilverMirrorReaction : MonoBehaviour
{
    [SerializeField] private Material silverMirrorMaterial;
    [SerializeField] private Material mainLiquidMaterial;
    private IBeaker _beaker;
    private float life = 3f;

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        Debug.Log("Gümüş Ayna Tepkimesi");

        // Beherin ana sıvısının materyalini değiştir
        _beaker.MainLiquid.material = silverMirrorMaterial;

        Destroy(gameObject, life);
    }

    private void OnDestroy()
    {
        // Temizlik: Sıvıyı eski materyaline döndür ve gizle
        if (_beaker != null && _beaker.MainLiquid != null)
        {
            _beaker.MainLiquid.material = mainLiquidMaterial;
            _beaker.MainLiquid.gameObject.SetActive(false);
        }
    }
}