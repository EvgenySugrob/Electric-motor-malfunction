using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaommetrBackPosition : MonoBehaviour, IInteractable
{
    [SerializeField] Megaommetr megaommetr;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline caseoutline;

    public void OnHoverEnter()
    {
        caseoutline.enabled = true;
    }

    public void OnHoverExit()
    {
        caseoutline.enabled = false;
    }

    public void OnInteract()
    {
        if(mouseCursorHandler.GetCurrentInstrument()==megaommetr.gameObject)
        {
            MegaommetrOnTable();
        }
    }

    private void MegaommetrOnTable()
    {
        megaommetr.PutOnTable();
        gameObject.SetActive(false);
    }

    public void OutlineForceDisable()
    {
        caseoutline.enabled = false;
    }
}
