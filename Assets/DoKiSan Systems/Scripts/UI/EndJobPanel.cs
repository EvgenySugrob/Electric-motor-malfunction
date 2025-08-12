using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndJobPanel : MonoBehaviour
{
    [Header("SymptomsCheck")]
    [SerializeField] private List<ToggleTrouble> toggleTroubles;
    [SerializeField] Color defaultTextColor;
    [SerializeField] Color correctColor;
    [SerializeField] Color wrongColor;

    [Header("Dropdown")]
    [SerializeField] TMP_Dropdown troubleDropdown;
    [SerializeField] GameObject correctTextContainer;
    [SerializeField] Image frameImage;
    [SerializeField] TMP_Text correctText;
    private int dropdownValue;

    [Header("ExitBt")]
    [SerializeField] Button endBt;
    [SerializeField] GameObject mainMenuExit;

    [Header("CrackToggle")]
    [SerializeField] CrackSpawn crackSpawn;
    [SerializeField] ToggleTrouble crack;

    private List<ToggleTrouble> isOnToggles = new();
    
    public void EndButtonClick()
    {
        endBt.interactable = false;
        troubleDropdown.interactable = false;
        mainMenuExit.SetActive(true);

        foreach (ToggleTrouble toggle in toggleTroubles)
        {
            toggle.SetInteractableState(false);
        }

        SetSelectingSymptoms();

        List<string> currentSympthoms = ScenarioManager.Instance.GetScenario().symptoms;

        SelectionCorrectOrNotSymptoms(currentSympthoms);
        CheckDropDown();
    }

    private void SetSelectingSymptoms()
    {
        foreach (ToggleTrouble tt in toggleTroubles)
        {
            if (tt.GetToggleState())
            {
                isOnToggles.Add(tt);
            }
        }
    }

    private void SelectionCorrectOrNotSymptoms(List<string> correctSymptoms)
    {
        foreach (ToggleTrouble toggle in isOnToggles)
        {
            if (correctSymptoms.Contains(toggle.GetSymptoms()))
            {
                toggle.SetTextColor(correctColor);
            }
            else
            {
                toggle.SetTextColor(wrongColor);
            }
        }

        foreach (ToggleTrouble toggle in toggleTroubles)
        {
            if(!toggle.GetToggleState())
            {
                if (correctSymptoms.Contains(toggle.GetSymptoms()))
                {
                    toggle.SetTextColor(correctColor);
                }
                else
                {
                    toggle.SetTextColor(defaultTextColor);
                }
            }
        }

        if (crackSpawn.GetCrackState())
        {
            if (crack.GetToggleState())
            {
                crack.SetTextColor(correctColor);
            }
            else
            {
                crack.SetTextColor(wrongColor);
            }
        }
        else
        {
            if (crack.GetToggleState())
            {
                crack.SetTextColor(wrongColor);
            }
        }
    }

    private void CheckDropDown()
    {
        MotorFaultType fault = ScenarioManager.Instance.GetScenario().faultType;

        Debug.Log($"faultINDEX - {(int)fault}  daunINDEX - {dropdownValue}");

        if ((int)fault == dropdownValue)
        {
            frameImage.color = correctColor;
        }
        else
        {
            frameImage.color = wrongColor;

            correctText.text = troubleDropdown.options[(int)fault].text;
            correctTextContainer.SetActive(true);
        }
    }

    public void SelectCurrentTrouble(int value)
    {
        dropdownValue = value;
    }
}
