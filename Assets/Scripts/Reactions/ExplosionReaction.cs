using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionReaction : MonoBehaviour
{
    private float life = 3f;
    private float destroyBeakerCounter = 2f;

    private MeshRenderer beakerMesh;
    
    private void Start()
    {
        Debug.Log("Patlama Tepkimesi");
        Destroy(gameObject, life);
        
        beakerMesh = BeakerManager.Instance.GetComponentInChildren<MeshRenderer>();
        DestroyBeaker();
    }

    private void Update() {
        destroyBeakerCounter -= Time.deltaTime;
        if(destroyBeakerCounter <= 0 && !beakerMesh.enabled)
            beakerMesh.enabled = true;
    }

    private void DestroyBeaker() {
        beakerMesh.enabled = false;
        BeakerManager.Instance.GetMainLiquidRenderer().gameObject.SetActive(false);
    }

}
