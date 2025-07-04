using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterSlotBackPosition : MonoBehaviour,IInteractable
{
    [Header("MainComponent")]
    [SerializeField] Multimeter multimeter;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline caseOutline;

    public void OnInteract()
    {
        if(mouseCursorHandler.GetCurrentInstrument()==multimeter.gameObject)
        {
            MultimeterOnTable();
        }
    }

    public void OnHoverEnter()
    {
        caseOutline.enabled = true;
    }

    public void OnHoverExit()
    {
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
}    