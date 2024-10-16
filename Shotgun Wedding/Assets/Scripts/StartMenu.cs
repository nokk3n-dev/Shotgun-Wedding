using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }
    public void StartGame()
    {
        // Load Level 1
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        // Load settings page (index 5)
        SceneManager.LoadScene(5); 
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        // Load menu page (index 0)
        SceneManager.LoadScene(0); 
    }

    public void Retry()
    {
        // Load Level 1
        SceneManager.LoadScene(1);
    }
}
