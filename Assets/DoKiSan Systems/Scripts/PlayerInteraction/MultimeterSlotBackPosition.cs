using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterSlotBackPosition : MonoBehaviour,IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] public bool IsHighlightedByScenario = false;

    [Header("MainComponent")]
    [SerializeField] Multimeter multimeter;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline caseOutline;
    [SerializeField] Color stepColorOutline;
    private Color defaultColorOutline;

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        defaultColorOutline = caseOutline.OutlineColor;
    }

    public void OnInteract()
    {
        if(mouseCursorHandler.GetCurrentInstrument()==multimeter.gameObject)
        {
            MultimeterOnTable();
        }
    }

    public void OnHoverEnter()
    {
        if (IsHighlightedByScenario)
        {
            caseOutline.OutlineColor = defaultColorOutline;
            return;
        }
        caseOutline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (IsHighlightedByScenario)
        {
            caseOutline.OutlineColor = stepColorOutline;
            return;
        }
        caseOutline.enabled = false;
    }

    private void MultimeterOnTable()
    {
        multimeter.PutOnTable();
        gameObject.SetActive(false);
    }

    public void OutlineForceDisable()
    {
        caseOutline.enabled = false;
    }

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = state;
        if (caseOutline != null)
        {
            if (state)
            {
                caseOutline.OutlineColor = stepColorOutline;
                caseOutline.enabled = true;
            }
            else
            {
                caseOutline.OutlineColor = defaultColorOutline;
                caseOutline.enabled = false;
            }
        }
    }
}    