using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    public static bool isPaused;
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
