using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverMirrorReaction : MonoBehaviour
{
    private MeshRenderer mainLiquid;
    private float life = 3f;
    private void Start()
    {
        Debug.Log("Gümüþ Ayna tepkimesi");

        mainLiquid = BeakerManager.Instance.GetMainLiquidRenderer();
        mainLiquid.gameObject.SetActive(false);

        Destroy(gameObject, life);
    }

}
