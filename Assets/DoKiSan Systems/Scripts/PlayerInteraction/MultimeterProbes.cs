using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterProbes : MonoBehaviour, IInteractable
{
    [Header("ProbeInteraction")]
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    [SerializeField] Color colorSelected;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private float durationAnim = 0.25f;
    [SerializeField] int targetLayerName;
    [SerializeField] int probeID;
    private int defaulLayerName;
    private Color startColor;

    [Header("MultimeterOrMegaommeter")]
    [SerializeField] bool isMegaommeter;

    [Header("Outline")]
    [SerializeField] Outline outline;

    [Header("MeasurementPoint")]
    [SerializeField] MeasurementPoint measurementPoint;
    [SerializeField] MeasurementManager measurementManager;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Transform currentParent;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private void Start()
    {
        defaulLayerName = transform.gameObject.layer;

        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        currentParent = transform.parent;

        startColor = outline.OutlineColor;
    }

    public void OnHoverEnter()
    {
        if(!isSelected)
            outline.enabled = true;
    }

    public void OnHoverExit()
    {
        if(!isSelected)
            outline.enabled = false;
    }

    public void OnInteract()
    {
        float currentTime = Time.time;

        if(currentTime -lastClickTime<=doubleClickThreshold)
        {
            if(measurementPoint != null)
            {
                measurementPoint.SetBusyState(false);
                measurementPoint = null;
            }

            StartCoroutine(ProbeBackToMultimeter());
            ForceOutlineDisable();
            measurementManager.ClearProbePoint(this);

            lastClickTime = 0;
            return;
        }

        lastClickTime = currentTime;

        

        if(!isSelected)
        {
            if(measurementPoint!=null)
            {
                measurementPoint.SetBusyState(false);
                measurementPoint = null;
            }

            isSelected= true;
            outline.OutlineColor = colorSelected;

            mouseCursorHandler.SetMultimeterProbe(this);
        }
        else
        {
            isSelected=false;
            outline.OutlineColor = startColor;

            mouseCursorHandler.SetMultimeterProbe(null);
        }
    }

    public void SetMeasuringPoint(Transform probePosition)
    {
        measurementManager.EnableTextMoveProbe();
        if (measurementPoint != null)
        {
            measurementPoint.SetBusyState(false);
        }
        ForceOutlineDisable();
        StartCoroutine(ProbeMoveToPoint(probePosition));
    }

    private IEnumerator ProbeMoveToPoint(Transform probePosition)
    {
        measurementPoint = probePosition.parent.GetComponent<MeasurementPoint>();

        isSelected = false;
        outline.OutlineColor = startColor;

        mouseCursorHandler.SetMultimeterProbe(null);

        transform.parent = null;
        transform.gameObject.layer = targetLayerName;

        foreach(Transform child in transform)
        {
            child.gameObject.layer = targetLayerName;
        }

        Debug.Log("LAYER " + gameObject.layer);

        measurementManager.ClearProbePoint(this);

        yield return ProbeMove(probePosition.position,probePosition.eulerAngles);

        measurementPoint.SetSelectPoint(this);
    }

    private YieldInstruction ProbeMove(Vector3 endPosition,Vector3 eulerAngle)
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(endPosition,durationAnim))
            .Join(transform.DORotate(eulerAngle,durationAnim))
            .Play()
            .WaitForCompletion();
    }

    public void ProbeBackAfterMeasurement()
    {
        StartCoroutine(ProbeBackToMultimeter());
    }

    private IEnumerator ProbeBackToMultimeter()
    {
        //measurementPoint.SetBusyState(false);
        transform.gameObject.layer = defaulLayerName;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = defaulLayerName;
        }
        transform.parent = currentParent;

        yield return ProbeMoveBack(startPosition,startRotation.eulerAngles);
    }

    private YieldInstruction ProbeMoveBack(Vector3 endPosition, Vector3 eulerAngle)
    {
        return DOTween.Sequence()
            .Append(transform.DOLocalMove(endPosition, durationAnim))
            .Join(transform.DOLocalRotate(eulerAngle, durationAnim))
            .Play()
            .WaitForCompletion();
    }

    public int GetProbeId()
    {
        return probeID;
    }

    public void SetMeasuramentPointReference(MeasurementPoint point)
    {
        measurementPoint = point;
    }

    private void ForceOutlineDisable()
    {
        outline.OutlineColor = startColor;
        outline.enabled = false;
    }

    public void ProbeStartPosition()
    {
        transform.parent = currentParent;
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    public void ForceCLear()
    {
        measurementManager.ClearProbePoint(this);
    }
}
