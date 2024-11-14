using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeGame : MonoBehaviour
{
    public string gameSceneName = "Lvl_1"; // Nama scene game yang sedang dimainkan

    // Fungsi ini dipanggil saat tombol Resume ditekan
    public void Resume()
    {
        Time.timeScale = 1f;  // Kembalikan waktu menjadi normal
        SceneManager.UnloadSceneAsync("Pause");  // Hapus scene pause
    }
}
