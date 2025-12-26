using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverMirrorReaction : MonoBehaviour
{
    [SerializeField] private Material silverMirrorMaterial;
    [SerializeField] private Material mainLiquidMaterial;
    private MeshRenderer mainLiquid;
    private float life = 3f;
    private void Start()
    {
        Debug.Log("Gümüþ Ayna tepkimesi");

        mainLiquid = BeakerManager.Instance.GetMainLiquidRenderer();
        mainLiquid.material = silverMirrorMaterial;
        //mainLiquid.gameObject.SetActive(false);

        Destroy(gameObject, life);
    }

    private void OnDestroy() {
        mainLiquid.material = mainLiquidMaterial;
        mainLiquid.gameObject.SetActive(false);
    }

}
