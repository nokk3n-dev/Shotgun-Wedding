using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public Image fianceHealthBar;
    [SerializeField] public Image FILHealthBar;
    [SerializeField] public GameObject CoconutPowerUp;
    [SerializeField] public GameObject RumPowerUp;

    private float fianceCurrentHeath = 100f;
    private float FILCurrentHeath = 100f;
    
    bool powerUpSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        CoconutPowerUp?.SetActive(false);
        RumPowerUp?.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FianceTakeDamage(float damage)
    {
        // Adjust the heath number
        fianceCurrentHeath -= damage;

        if (fianceCurrentHeath <= 0)
        {
            Lose();
        }
        else
        {
            // Fill the healthbar to the % of current health
            fianceHealthBar.fillAmount = fianceCurrentHeath / 100f;
            CheckPowerUpSpawnCondition();
        }
    }

    public void FILTakeDamage(float damage)
    {
        // Adjust the heath number
        FILCurrentHeath -= damage;

        if (FILCurrentHeath <= 0)
        {
            Win();
        }
        else
        {
            // Fill the healthbar to the % of current health
            FILHealthBar.fillAmount = FILCurrentHeath / 100f;
            CheckPowerUpSpawnCondition();
        }
    }

    private void Lose()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            // Set the savedLevel to the Bar Fight
            PlayerPrefs.SetInt("savedLevel", 2);
        }

        // Load Lose screen page
        SceneManager.LoadScene("Lose"); 
    }

    private void Win()
    {
        // Increment the saved level
        PlayerPrefs.SetInt("savedLevel", PlayerPrefs.GetInt("savedLevel") + 1);

        // Load the win screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void CheckPowerUpSpawnCondition()
    {
        if (!powerUpSpawned)
        {
            if (fianceCurrentHeath <= 50f)
            {
                CoconutPowerUp?.SetActive(true);
                powerUpSpawned = true;
            }
            else if (FILCurrentHeath <= 50f)
            {
                RumPowerUp?.SetActive(true);
                powerUpSpawned = true;
            }
        }
    }
}
