using UnityEngine;
using System.Collections.Generic;

public class LiquidMixerVisualizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer mainLiquid;

    public void UpdateMixedVisuals(Color mixedColor, int count)
    {
        mainLiquid.gameObject.SetActive(true);
        mainLiquid.material.color = mixedColor;
        // Sıvı miktarını artır
        mainLiquid.transform.localScale = new Vector3(1, count * 1, 1);
    }
}