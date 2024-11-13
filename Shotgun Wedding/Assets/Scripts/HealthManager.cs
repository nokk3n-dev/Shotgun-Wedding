using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public Image fianceHealthBar;
    [SerializeField] public Image FILHealthBar;

    public float fianceCurrentHeath = 100f;
    public float FILCurrentHeath = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    private void Lose()
    {
        // Set the savedLevel to the Bar Fight
        PlayerPrefs.SetInt("savedLevel", 2);

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
}
