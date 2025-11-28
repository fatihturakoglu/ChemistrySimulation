using UnityEngine;
using Cinemachine; // Cinemachine kütüphanesini eklemeyi unutma!
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera masaKamerasi;
    [SerializeField] private CinemachineVirtualCamera dolapKamerasi;
    [SerializeField] private Button LookTableBtn;
    [SerializeField] private Button LookCupboardBtn;
    // Başlangıçta masaya baksın
    
    private void Start()
    {
        MasayaDon();
        LookTableBtn.gameObject.SetActive(false);
        LookCupboardBtn.gameObject.SetActive(true);
    }

    public void DolabaBak()
    {
        // Dolap kamerasının önceliğini artır, diğerini düşür
        dolapKamerasi.Priority = 11;
        masaKamerasi.Priority = 10;
        LookTableBtn.gameObject.SetActive(true);
        LookCupboardBtn.gameObject.SetActive(false);
    }

    public void MasayaDon()
    {
        // Masa kamerasının önceliğini artır
        masaKamerasi.Priority = 11;
        dolapKamerasi.Priority = 10;
        LookTableBtn.gameObject.SetActive(false);
        LookCupboardBtn.gameObject.SetActive(true);
    }
}