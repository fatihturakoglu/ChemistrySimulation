using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private ParticleSystem suSistemi;

    void Start()
    {
        suSistemi = GetComponentInChildren<ParticleSystem>();
        var emission = suSistemi.emission;
        emission.enabled = false; // Ba?lang?çta akmas?n
    }

    // Envanter sistemin bu fonksiyonu ça??racak
    public void SuyuBaslat()
    {
        var emission = suSistemi.emission;
        emission.enabled = true;
    }

    public void SuyuDurdur()
    {
        var emission = suSistemi.emission;
        emission.enabled = false;
    }
}
