using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPoint : MonoBehaviour,IInteractable
{
    public string pointId;
    [SerializeField] Transform probePosition;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Outline outline;
    private MultimeterProbes currentProbe;
    private bool isBusy = false;

    private void Start()
    {
        probePosition = transform.GetChild(0).transform;
    }

    public void OnHoverEnter()
    {
        if(mouseCursorHandler.GetMultimeterProbe()!=null)
            outline.enabled = true;
    }

    public void OnHoverExit()
    {
        if(mouseCursorHandler.GetMultimeterProbe()!=null)
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
            }
        }
    }
}
