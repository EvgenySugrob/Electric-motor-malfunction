using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTrouble : MonoBehaviour
{
    [SerializeField] Toggle toggleTruble;
    [SerializeField] string symptoms;
    [SerializeField] TMP_Text description;

    private void Start()
    {
        toggleTruble = transform.GetComponent<Toggle>();
    }

    public bool GetToggleState()
    {
        return toggleTruble.isOn;
    }

    public string GetSymptoms()
    {
        return symptoms;
    }

    public void SetTextColor(Color color)
    {
        description.color = color;
    }

    public void SetInteractableState(bool isState)
    {
        toggleTruble.interactable = isState;
    }
}
