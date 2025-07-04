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
        isSelected = false;
        outline.OutlineColor = startColor;

        mouseCursorHandler.SetMultimeterProbe(null);

        transform.parent = null;
        transform.gameObject.layer = targetLayerName;
        yield return ProbeMove(probePosition.position,probePosition.eulerAngles);
    }

    private YieldInstruction ProbeMove(Vector3 endPosition,Vector3 eulerAngle)
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(endPosition,durationAnim))
            .Join(transform.DORotate(eulerAngle,durationAnim))
            .Play()
            .WaitForCompletion();
    }
}
