using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    // This is a reference to the Load Game Button
    [SerializeField] Button LoadGameButton;

    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }

        if (PlayerPrefs.HasKey("savedLevel"))
        {
            LoadGameButton.interactable = true;
        }
    }

    // When this is called, it will reset all the progress of a previous load state
    public void NewGame()
    {
        // This will autosave in case the player doesn't pause and save the game.
        PlayerPrefs.SetInt("savedLevel", 1);

        // Load the tutorial level
        SceneManager.LoadScene("Tutorial");
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("savedLevel"));
    }

    public void OpenSettings()
    {
        // Load settings page (index 5)
        SceneManager.LoadScene("Settings"); 
    }

    public void Quit()
    {
        // This will ensure that we reset the load file if we already won the game
        if (SceneManager.GetActiveScene().name == "Win")
        {
            PlayerPrefs.DeleteKey("savedLevel");
        }
        
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        // This will ensure that we reset the load file if we already won the game
        if (SceneManager.GetActiveScene().name == "Win")
        {
            PlayerPrefs.DeleteKey("savedLevel");
        }

        // Load menu page (index 0)
        SceneManager.LoadScene("Main Menu"); 
    }

    public void Retry()
    {
        // Load the level
        SceneManager.LoadScene(PlayerPrefs.GetInt("savedLevel"));
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main Menu");
    }
}
