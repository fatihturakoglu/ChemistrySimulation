using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLabObject", menuName = "Lab System/LabObject")]
public class LabObjectSO : ScriptableObject
{
    [Header("Malzeme Özellikleri")]
    public string objectName;
    public Transform prefab;
    public bool isReusable; 
    public bool isLiquid;
    public bool hasMultipleMeshes;
    public Color color;

    [Tooltip("Su = 1.0, Bal = 1.4, Yağ = 0.9 gibi değerler verin.")]
    public float density;
}
    