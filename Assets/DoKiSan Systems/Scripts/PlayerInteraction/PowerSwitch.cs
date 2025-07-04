using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerSwitch : MonoBehaviour, IInteractable
{
    [Header("Main Setting")]
    [SerializeField] MotorController motorController;
    [SerializeField] AssembleManager assembleManager;
    [SerializeField] Outline outline;
    [SerializeField] float durationAnimation = 0.5f;
    [SerializeField] Vector3 angleRotate = new Vector3(0,0,0);
    private bool isOn = false;

    public void OnHoverEnter()
    {
        outline.enabled = true;
    }

    public void OnHoverExit()
    {
        outline.enabled = false;
    }

    public void OnInteract()
    {
        if(assembleManager.IsListFreeOfObject())
        StartCoroutine(SwitchRotate());
    }

    private IEnumerator SwitchRotate()
    {
        yield return transform.DOLocalRotate(isOn ? angleRotate : -angleRotate, durationAnimation, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutQuad)
            .Play()
            .WaitForCompletion();

        isOn = !isOn;
        motorController.ActiveMotorButtonClick();
    }

    
}
