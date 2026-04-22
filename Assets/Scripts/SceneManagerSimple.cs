using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için şart

public class SceneManagerSimple : MonoBehaviour
{
    public void SonrakiDeneyeGec()
    {
        // Mevcut aktif sahnenin numarasını al
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Bir sonraki sahneye git
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void SahneyeGit(string sahneAdi)
    {
        // Eğer istersen isme göre de geçiş yapabilirsin
        SceneManager.LoadScene(sahneAdi);
    }
}