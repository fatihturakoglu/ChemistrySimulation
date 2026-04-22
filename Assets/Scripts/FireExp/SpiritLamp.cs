using UnityEngine;
using System;

public class SpiritLamp : MonoBehaviour
{
    public event EventHandler OnLightAction; // BeakerManager'?n dinledi?i olay
    [SerializeField] private GameObject fireVFX;
    private bool isLit = false;

    public static SpiritLamp Instance { get; private set; }
    private void Awake() { Instance = this; }

    private void OnMouseDown()
    {
        isLit = !isLit;
        if (fireVFX != null) fireVFX.SetActive(isLit);

        if (isLit)
        {
            // Ate? yand???nda BeakerManager'a haber ver
            OnLightAction?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsLit() => isLit;
}