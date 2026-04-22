using UnityEngine;
using Unity.VisualScripting;
using Cinemachine;

public class CameraFcous : MonoBehaviour
{
    [Header("Zoom Ayarları")]
    public float normalFOV = 60f;
    public float focusFOV = 30f;
    public float smoothSpeed = 5f;

    private CinemachineVirtualCamera activeCam;

    void Update()
    {
        // 1. Sahnedeki aktif sanal kamerayı bul (Performans için gerekirse cache'lenebilir)
        activeCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        if (activeCam == null) return;

        // 2. Hedef FOV belirle
        float targetFOV = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                          ? focusFOV
                          : normalFOV;

        // 3. Yumuşak geçişle FOV değerini uygula
        activeCam.m_Lens.FieldOfView = Mathf.Lerp(activeCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}