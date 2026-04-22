using UnityEngine;
using VContainer;
using System.Collections;

public class TitrasyonReaction : MonoBehaviour
{
    private IBeaker _beaker;
    [SerializeField] private Color targetPink = new Color(1f, 0.07f, 0.57f, 0.6f);

    [Inject]
    public void Construct(IBeaker beaker)
    {
        _beaker = beaker;
    }

    private void Start()
    {
        if (_beaker != null) StartCoroutine(AnidenPembeles());
        Destroy(gameObject, 4f);
    }

    private IEnumerator AnidenPembeles()
    {
        MeshRenderer liquid = _beaker.MainLiquid;
        Color startColor = liquid.material.color;
        float elapsed = 0;

        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            liquid.material.color = Color.Lerp(startColor, targetPink, elapsed);
            yield return null;
        }
    }
}