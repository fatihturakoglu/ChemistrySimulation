using UnityEngine;
using System.Collections;

public class IodineSublimationReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem gasParticle;
    [SerializeField] private float dissolveDuration = 5f;

    private IEnumerator Start()
    {
        if (gasParticle != null) gasParticle.Play();

        // Beaker'ın listeyi temizlemesine fırsat tanımak için 1 frame bekle
        yield return new WaitForEndOfFrame();

        Transform iodineParent = FireExperimentBeaker.Instance.GetFirstIngredient();

        if (iodineParent != null)
        {
            Debug.Log("Süblimleşme (Katıdan Gaza Geçiş) Başlıyor...");
            yield return StartCoroutine(DissolveAllIodineParts(iodineParent));
        }
        else
        {
          //  Debug.LogWarning("Hata: Reaksiyon başladı ama iyot bulunamadı!");
        }

        // Gaz efektini bir süre sonra yok et
        Destroy(gameObject, 10f);
    }

    private IEnumerator DissolveAllIodineParts(Transform parent)
    {
        Vector3 startScale = parent.localScale;
        float elapsed = 0;

        while (elapsed < dissolveDuration)
        {
            if (parent == null) yield break;

            elapsed += Time.deltaTime;
            parent.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / dissolveDuration);
            yield return null;
        }

        if (parent != null) Destroy(parent.gameObject);
        Debug.Log("İyot tamamen süblimleşti.");
    }
}