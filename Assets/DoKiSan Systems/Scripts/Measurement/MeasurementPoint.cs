using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPoint : MonoBehaviour,IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] private string triggerEventName;
    [SerializeField] public bool IsHighlightedByScenario = false;

    public string pointId;
    [SerializeField] MeasurementManager measurementManager;
    [SerializeField] Transform probePosition;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline outline;
    [SerializeField] Color stepColorOutline;
    [SerializeField] private MultimeterProbes currentProbe;
    private bool isBusy = false;
    private Color defaultColorOutline;

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        defaultColorOutline = outline.OutlineColor;
        probePosition = transform.GetChild(0).transform;
    }

    public void OnHoverEnter()
    {
        if (IsHighlightedByScenario)
        {
            outline.OutlineColor = defaultColorOutline;
            return;
        }
            

        if(mouseCursorHandler.GetMultimeterProbe()!=null)
            outline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (IsHighlightedByScenario)
        {
            outline.OutlineColor = stepColorOutline;
            return;
        }
            
        if (mouseCursorHandler.GetMultimeterProbe()!=null)
            outline.enabled = false;
    }

    public void OnInteract()
    {
        if(mouseCursorHandler.GetMultimeterProbe()!=null)
        {
            if(!isBusy)
            {
                currentProbe = mouseCursorHandler.GetMultimeterProbe();

                currentProbe.SetMeasuringPoint(probePosition);

                isBusy= true;

                InstructionManager.Instance.OnEventTriggered(triggerEventName, 0f);

                ForceDisableOutline();
            }
        }
    }

    public void SetSelectPoint(MultimeterProbes probe)
    {
        currentProbe = probe;
        currentProbe.SetMeasuramentPointReference(this);

        measurementManager.SetProbePoint(probe, pointId);
    }

    public bool GetBusyState()
    {
        return isBusy;
    }
    public void SetBusyState(bool isState)
    {
        isBusy = isState;
    }

    private void ForceDisableOutline()
    {
        if (IsHighlightedByScenario)
        {
            outline.OutlineColor = stepColorOutline;
            return;
        }

        outline.enabled= false;
    }

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = state;
        if (outline!=null)
        {
            if(state)
            {
                outline.OutlineColor = stepColorOutline;
                outline.enabled = true;
            }
            else
            {
                outline.OutlineColor = defaultColorOutline;
                outline.enabled = false;
            }
        }
    }
}
