using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MultimetrModeWheel : MonoBehaviour, IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] bool IsHighlightedByScenario;

    [Header("ModeWheel")]
    [SerializeField] Outline outline;
    [SerializeField] Color stepColorOutline;
    [SerializeField] Vector3 eulerRotation;
    [SerializeField] Vector3 eulerRotationBack;
    [SerializeField] float duration = 0.25f;
    private BoxCollider boxCollider;
    private Color defaultColorOutline;

    [Header("DisplayMultimeter")]
    [SerializeField] TMP_Text numbersText;
    [SerializeField] GameObject renderPart;
    private bool isOn = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        defaultColorOutline = outline.OutlineColor;
    }

    public void WheelColliderState(bool isState)
    {
        boxCollider.enabled = isState;
    }

    public void OnHoverEnter()
    {
        if (IsHighlightedByScenario)
        {
            outline.OutlineColor = defaultColorOutline;
            return;
        }
        outline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (IsHighlightedByScenario)
        {
            outline.OutlineColor = stepColorOutline;
            return;
        }
        outline.enabled = false;
    }

    public void OnInteract()
    {
        if(isOn)
        {
            isOn= false;
        }
        else
        {
            isOn= true;
        }
        StartCoroutine(RotationModeWheel(isOn));
    }

    private IEnumerator RotationModeWheel(bool isState)
    {
        if(isState)
        {
            yield return transform.DOLocalRotate(eulerRotation, duration)
                                .Play();
        }
        else
        {
            yield return transform.DOLocalRotate(eulerRotationBack, duration)
                                .Play();
        }
        renderPart.SetActive(isOn);
    }

    public void DisableAndWheelOnStart()
    {
        if(isOn)
        {
            isOn = false;
            StartCoroutine(RotationModeWheel(isOn));
        }
            
        renderPart.SetActive(false);
    }

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = true;
        if (outline != null)
        {
            if (state)
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
