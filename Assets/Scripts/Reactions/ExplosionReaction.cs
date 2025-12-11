using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    private float life = 3f;
    
    
    void Start()
    {
        Debug.Log("BUM");
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject, life); //oluþturulduktan 3sn sonra yok olsun
    }

}
