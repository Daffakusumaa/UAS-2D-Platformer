using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public string pauseSceneName = "Pause"; // Nama scene pause

    void Update()
    {
        // Cek apakah tombol Esc ditekan untuk menjeda permainan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pindah ke scene pause
            Time.timeScale = 0f;  // Hentikan permainan
            SceneManager.LoadScene(pauseSceneName, LoadSceneMode.Additive);  // Muat scene pause tanpa menutup scene game
        }
    }
}
