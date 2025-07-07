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
    private Color startColor;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private float durationAnim = 0.25f;
    [SerializeField] LayerMask targetLayerName;
    private LayerMask defaulLayerName;

    [Header("Multimeter")]
    [SerializeField] Multimeter multimeter;

    [Header("Outline")]
    [SerializeField] Outline outline;

    [Header("MeasurementPoint")]
    [SerializeField] MeasurementPoint measurementPoint;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Transform currentParent;

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
        if(!isSelected)
        {
            if(measurementPoint!=null)
            {
                measurementPoint.SetBusyState(false);
                //добавить сброс точки на проверку.
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
        yield return ProbeMove(probePosition.position,probePosition.eulerAngles);

        measurementPoint.SetSelectPoint();
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
        measurementPoint.SetBusyState(false);
        transform.gameObject.layer = defaulLayerName;
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
}
