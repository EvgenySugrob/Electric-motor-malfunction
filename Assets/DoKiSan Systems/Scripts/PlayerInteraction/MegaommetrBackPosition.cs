using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaommetrBackPosition : MonoBehaviour, IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] public bool IsHighlightedByScenario = false;

    [SerializeField] Megaommetr megaommetr;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline caseoutline;
    [SerializeField] Color stepColorOutline;
    private Color defaultColorOutline;

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        defaultColorOutline = caseoutline.OutlineColor;
    }

    public void OnHoverEnter()
    {
        if (IsHighlightedByScenario)
        {
            caseoutline.OutlineColor = defaultColorOutline;
            return;
        }
        caseoutline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (IsHighlightedByScenario)
        {
            caseoutline.OutlineColor = stepColorOutline;
            return;
        }
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

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = state;
        if (caseoutline != null)
        {
            if (state)
            {
                caseoutline.OutlineColor = stepColorOutline;
                caseoutline.enabled = true;
            }
            else
            {
                caseoutline.OutlineColor = defaultColorOutline;
                caseoutline.enabled = false;
            }
        }
    }
}
