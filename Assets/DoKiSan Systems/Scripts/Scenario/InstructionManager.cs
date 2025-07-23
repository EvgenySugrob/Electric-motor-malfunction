using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text instructionText;

    private List<InstructionStep> steps;
    private int currentStep = 0;
    private HashSet<string> triggeredEvents = new();
    [SerializeField] private GameObject instructionalPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void StartInstructions(List<InstructionStep> instructionSteps)
    {
        if (instructionSteps == null || instructionSteps.Count == 0)
        {
            Debug.LogError("InstructionManager: Шаги не заданы или пусты.");
            return;
        }

        if(instructionalPanel==null)
        {
            instructionalPanel = GameObject.FindGameObjectWithTag("InstructionalPanel");
        }

        instructionalPanel.SetActive(true);
        steps = instructionSteps;
        currentStep = 0;
        ShowCurrentStep();
    }

    public void OnEventTriggered(string eventKey, float waitTimeToNextStep)
    {
        if (currentStep >= steps.Count)
            return;

        var step = steps[currentStep];

        triggeredEvents.Add(eventKey);

        bool allEventsTriggered = step.requiredEvents.All(evt => triggeredEvents.Contains(evt));

        if (allEventsTriggered)
        {
            triggeredEvents.Clear();
            AdvanceStep(waitTimeToNextStep);
        }
    }

    private void AdvanceStep(float waitTimeToNextStep)
    {
        foreach (var id in steps[currentStep].objectIDsToHighlight)
        {
            HighlightController.Instance.UnhighlightByID(id);
        }

        currentStep++;

        StartCoroutine(WaitSecondToNextStep(waitTimeToNextStep));
        //ShowCurrentStep();
    }

    private IEnumerator WaitSecondToNextStep(float waitTimeToNextStep)
    {
        instructionText.text = "";

        if(instructionalPanel!=null && waitTimeToNextStep>0)
        {
            instructionalPanel.SetActive(false);
        }

        yield return new WaitForSeconds(waitTimeToNextStep);

        instructionalPanel.SetActive(true);
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= steps.Count)
        {
            instructionText.text = "Лабораторная работа завершена.";
            return;
        }

        InstructionStep step = steps[currentStep];
        instructionText.text = step.stepText;

        if (HighlightController.Instance == null)
        {
            Debug.LogError("HighlightController.Instance is null — подсветка невозможна.");
            return;
        }

        if (step.objectIDsToHighlight == null || step.objectIDsToHighlight.Count == 0)
        {
            Debug.LogWarning($"Шаг {currentStep}: objectIDsToHighlight пуст.");
            return;
        }

        foreach (var id in step.objectIDsToHighlight)
        {
            Debug.Log($"Попытка подсветить объект с ID: {id}");
            HighlightController.Instance.HighlightByID(id);
        }
        //if (currentStep >= steps.Count)
        //{
        //    instructionText.text = "Лабораторная работа завершена.";
        //    return;
        //}

        //InstructionStep step = steps[currentStep];
        //instructionText.text = step.stepText;

        //foreach (var id in step.objectIDsToHighlight)
        //{
        //    HighlightController.Instance.HighlightByID(id);
        //}
    }

}
