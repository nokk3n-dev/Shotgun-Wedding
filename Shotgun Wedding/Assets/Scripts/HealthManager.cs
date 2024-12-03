using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public Image fianceHealthBar;
    [SerializeField] public Image FILHealthBar;
    [SerializeField] public Image fianceStaminaBar;
    [SerializeField] public GameObject CoconutPowerUp;
    [SerializeField] public GameObject RumPowerUp;

    private float fianceCurrentHeath = 100f;
    private float fianceMaxHealth = 100f;
    private float FILCurrentHeath = 100f;
    private float FILMaxHealth = 100f;
    private float fianceCurrentStamina = 100f;
    private float fianceMaxStamina = 100f;
    
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

    /************************************************************
     * FianceHeal
     ************************************************************
     * Description: This function will heal the fiance the amount
     * passed in through the parameter.
     ************************************************************
     * Parameters: 
     * heal - This will be how much health they gain back
     ************************************************************
     * Returns: None
     ************************************************************/
    public void FianceHeal(float heal)
    {
        // Adjust the health number, ensuring it doesn't exceed max health
        fianceCurrentHeath = Mathf.Min(fianceCurrentHeath + heal, fianceMaxHealth);
        // Fill the healthbar to the % of current health
        fianceHealthBar.fillAmount = fianceCurrentHeath / 100f;
    }

    /************************************************************
     * FILHeal
     ************************************************************
     * Description: This function will heal the FIL the amount
     * passed in through the parameter.
     ************************************************************
     * Parameters: 
     * heal - This will be how much health they gain back
     ************************************************************
     * Returns: None
     ************************************************************/
    public void FILHeal(float heal)
    {
        // Adjust the health ensuring it doesn't exceed max health
        FILCurrentHeath = Mathf.Min(FILCurrentHeath + heal, FILMaxHealth);
        // Fill the healthbar to the % of current health
        fianceHealthBar.fillAmount = fianceCurrentHeath / 100f;
    }

    /************************************************************
     * FianceExhaust
     ************************************************************
     * Description: This function will decrement the stamina 
     * of the fiance. If the stamina drops to 0, then they will
     * not be able to throw any punches or block until it fills
     * back up.
     ************************************************************
     * Parameters: 
     * energy - This will be how much stamina they lose
     ************************************************************
     * Returns: None
     ************************************************************/
    public void FianceExhaust(float energy)
    {
        fianceCurrentStamina -= energy;
        fianceStaminaBar.fillAmount = fianceCurrentStamina / 100f;

        if (fianceCurrentStamina <= 0)
        {
            fianceCurrentStamina = 0;
            
            // fiance.isExhausted = true; - Need to implement something like this
        }
    }

    /************************************************************
     * GetFianceHealth
     ************************************************************
     * Description: This helper function will return the 
     * current health of the fiance
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: float
     ************************************************************/
    public float GetFianceHealth()
    {
        return fianceCurrentHeath;
    }

    /************************************************************
     * GetFILHealth
     ************************************************************
     * Description: This helper function will return the 
     * current health of the FIL
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: float
     ************************************************************/
    public float GetFILHealth()
    {
        return FILCurrentHeath;
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
