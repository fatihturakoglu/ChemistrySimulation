using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionReaction : MonoBehaviour
{
    private float life = 3f;
    
    void Start()
    {
        Debug.Log("Patlama Tepkimesi");
        Destroy(gameObject, life); 
    }

}
