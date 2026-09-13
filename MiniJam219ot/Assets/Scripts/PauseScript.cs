using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    [SerializeField]GameObject pickingMenu;
    public static bool isPaused;
    [SerializeField] GameObject pauseMenu;


    void Start()
    {
        //pickingMenu.gameObject.SetActive(true);
        //SimplePause();
        WeaponChoice();
    }
    public void WeaponChoice()
    {
        pickingMenu.gameObject.SetActive(true);
        SimplePause();
    }
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

    //MISC STUFF DOWN HERE
    public void SimplePause()
    {
        Time.timeScale = 0;
    }
    public void SimpleResume()
    {
        Time.timeScale = 1;
    }
}
