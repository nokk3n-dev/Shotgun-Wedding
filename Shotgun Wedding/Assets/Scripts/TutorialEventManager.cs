using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialEventManager : MonoBehaviour
{
    // Enum to keep track of the different steps in the tutorials
    private enum TutorialStep 
    {
        Welcome,
        Movement,
        Blocking,
        Fighting,
        PowerUps,
        Enemy,
        End
    }

    // This variable will keep track of the current step we are on
    private TutorialStep currentStep;

    // This array will hold the Text objects for each step
    public TextMeshProUGUI[] stepTexts;

    // This holds all of the tutorial UI
    [SerializeField] GameObject tutorialUI;

    // This holds a reference to the FIL game object so that he is static until the tutorial is complete
    [SerializeField] GameObject FIL_Reference;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = TutorialStep.Welcome;
        UpdateStepDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NextStep();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PreviousStep();
        }
    }

    private void NextStep()
    {
        if (currentStep < TutorialStep.Enemy)
        {
            currentStep++;
            UpdateStepDisplay();
        } else if ((currentStep+1) == TutorialStep.End)
        {
            FIL_Reference.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            tutorialUI.SetActive(false);
        }
    }

    private void PreviousStep()
    {
        if (currentStep > TutorialStep.Welcome)
        {
            currentStep--;
            UpdateStepDisplay();
        }
    }

    private void UpdateStepDisplay()
    {
        // Hide all the text object initially
        foreach (TextMeshProUGUI text in stepTexts)
        {
            text.gameObject.SetActive(false);
        }

        // Activate the text for the current step
        stepTexts[(int)currentStep].gameObject.SetActive(true);

    }
}
