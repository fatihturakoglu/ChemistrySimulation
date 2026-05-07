using UnityEngine;
using System.Collections.Generic;

public enum LabAnimationType { None, LiquidPour, SolidSpill }

[CreateAssetMenu(fileName = "NewLabObject", menuName = "Lab System/LabObject")]
public class LabObjectSO : ScriptableObject
{
    [Header("Malzeme Özellikleri")]
    public string objectName;
    public Transform prefab;

    [Header("Kimyasal Davranış")]
    public bool isLiquid;
    public bool isReusable;
    // Maddenin çözünebildiği sıvıların isim listesi (Örn: "Water")
    public List<string> solubleIn;

    [Header("Görsel Ayarlar")]
    public Transform visualPrefabInBeaker;
    public LabAnimationType animationType;
    public Color color;
    public float density;

    [Header("Eklenen sıvının rengini de derecede etkileyecek")]
    [Range(0f, 1f)] public float colorIntensity = 0.5f;
}