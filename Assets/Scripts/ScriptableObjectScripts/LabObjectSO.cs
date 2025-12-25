using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LabObjectSO : ScriptableObject
{
    [Header("Malzeme Özellikleri")]
    public string objectName;
    public Transform prefab;
    public bool isReusable; //ingredientmanager'da kullanmadýk, orada rb olup olmamasýna göre ayarladýk
    public bool isLiquid;
    public bool hasMultipleMeshes;

    public Color color;
}
