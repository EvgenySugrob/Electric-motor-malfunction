using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AnimationDisassembly : MonoBehaviour
{
    [Header("WiresAndNuts")]
    [SerializeField] bool isWiresAndNuts;
    [SerializeField] List<WireMotor> wiresMotorList;
    [SerializeField] List<WireBlocks> wiresBlocksList;
    [SerializeField] List<NutsRotate> nutsRotateList;
    [SerializeField] List<BoxCollider> collidersMeasuringPoints;
    [SerializeField] MouseCursorHandler mouseCursorHandler;

    [Header("Brushes")]
    [SerializeField] bool isBrushes;

    [Header("Points")]
    [SerializeField] List<Transform> points;

    [Header("Child enable")]
    [SerializeField] List<GameObject> childEnable;

    [Header("Setting")]
    [SerializeField] Vector3 screwLocalRotate = new Vector3(0,0,-720f);
    [SerializeField] float durationBetween = 1f;
    [SerializeField] float durationEnd = 1f;
    [SerializeField] bool isScrew = false;
    [SerializeField] bool isDisableObject = false;
    [SerializeField] Ease ease;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private Sequence movePointSequence;
    private InteractableObject interactableObject;
    private Coroutine waitForDisableChild;

    [Header("MainVentylator")]
    [SerializeField] Transform mainVentylator;
    [SerializeField] Transform hideParent;
    [SerializeField] Transform mainVentylatorParent; 

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;

        interactableObject= GetComponent<InteractableObject>();
    }

    public void DisassemblyAnimationStart()
    {
        if (movePointSequence != null)
            movePointSequence.Kill();

        if (isScrew)
        {
            ScrewDisassembly();
        }
        else
        {
            DefaultDetailDisassembly();
        }
    }

    public void AssemblyAnimationStart()
    {
        if(movePointSequence!=null)
            movePointSequence.Kill();

        if (isScrew)
        {
            ScrewAssembly();
        }
        else
        {
            DefaultDetailAssembly();
        }
    }

    private void DefaultDetailDisassembly()
    {
        if(isWiresAndNuts)
        {
            foreach (NutsRotate nut in nutsRotateList)
            {
                nut.RotationNuts(false);
            }

            foreach (WireBlocks wireBlocks in wiresBlocksList)
            {
                wireBlocks.RemoveWires(false);
            }

            foreach (WireMotor wireMotor in wiresMotorList)
            {
                wireMotor.RemoveWireMotor(false);
            }

            foreach (BoxCollider collider in collidersMeasuringPoints)
            {
                collider.enabled = true;
            }
        }

        if (mainVentylator != null)
        {
            Debug.Log("mainVentylator != null");
            mainVentylator.parent = hideParent;
            childEnable[0].transform.rotation = mainVentylator.rotation;
        }

        if (childEnable.Count>0)
        {
            foreach (GameObject childObj in childEnable)
            {
                childObj.SetActive(true);
            }
        }

        GameObject[] disableObject = interactableObject.ObjectsToDisable;
        for (int i = 0; i < disableObject.Length; i++)
        {
            disableObject[i].SetActive(false);
        }


        movePointSequence = DOTween.Sequence();
        for (int i = 0; i < points.Count; i++)
        {
            movePointSequence
                .Append(transform.DOMove(points[i].position, durationEnd))
                .Join(transform.DORotate(points[i].eulerAngles, durationEnd))
                .SetEase(ease);
        }
        movePointSequence
            .SetEase(ease)
            .Play();
    }

    private void ScrewDisassembly()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (i==0)
            {
                movePointSequence = DOTween.Sequence()
                .Append(transform.DOMove(points[i].position, durationBetween))
                .Join(transform.DOLocalRotate(screwLocalRotate, durationBetween / 1.2f, RotateMode.LocalAxisAdd))
                .SetEase(ease);
            }
            else
            {
                movePointSequence
                .Append(transform.DOMove(points[i].position, durationEnd))
                .Join(transform.DORotate(points[i].eulerAngles, durationEnd))
                .SetEase(ease);
            }
        }
        movePointSequence
            .SetEase(ease)
            .Play();
    }

    private void ScrewAssembly()
    {
        movePointSequence = DOTween.Sequence();
        for (int i = points.Count-2; i >= 0; i--)
        {
            movePointSequence
                .Append(transform.DOMove(points[i].position, durationEnd))
                .Join(transform.DORotate(points[i].eulerAngles, durationEnd))
                .SetEase(ease);
        }
        movePointSequence
                .Append(transform.DOMove(startPosition, durationBetween))
                .Join(transform.DOLocalRotate((-1) * screwLocalRotate, durationBetween / 1.2f, RotateMode.LocalAxisAdd))
                .SetEase(ease);

        movePointSequence
            .SetEase(ease)
            .Play();
    }

    private void DefaultDetailAssembly()
    {
        if (isWiresAndNuts)
        {
            foreach (NutsRotate nut in nutsRotateList)
            {
                nut.RotationNuts(true);
            }

            foreach (WireBlocks wireBlocks in wiresBlocksList)
            {
                wireBlocks.RemoveWires(true);
            }

            foreach (WireMotor wireMotor in wiresMotorList)
            {
                wireMotor.RemoveWireMotor(true);
            }

            foreach (BoxCollider collider in collidersMeasuringPoints)
            {
                collider.enabled = false;
            }

            if(mouseCursorHandler.GetCurrentInstrument()!=null)
            {
                GameObject instrument = mouseCursorHandler.GetCurrentInstrument();
                if(instrument.GetComponent<Multimeter>())
                {
                    instrument.GetComponent<Multimeter>().ProbesBack();
                }
                else if(instrument.GetComponent<Megaommetr>())
                {
                    instrument.GetComponent<Megaommetr>().ForceProbeBack();
                }
            }
        }

        if (childEnable!=null)
        {
            waitForDisableChild = StartCoroutine(WaitForChildDisable());
        }

        movePointSequence = DOTween.Sequence();
        for (int i = points.Count-2; i >= 0; i--)
        {
            movePointSequence
                .Append(transform.DOMove(points[i].position, durationEnd))
                .Join(transform.DORotate(points[i].eulerAngles, durationEnd))
                .SetEase(ease);
        }
        movePointSequence.
            Append(transform.DOMove(startPosition,durationBetween))
            .Join(transform.DORotate(startRotation,durationBetween))
            .SetEase(ease);

        movePointSequence
            .SetEase(ease)
            .Play();
    }

    private IEnumerator WaitForChildDisable()
    {
        float time = points.Count;

        yield return new WaitForSeconds(time);

        InteractableObject interact = GetComponent<InteractableObject>();

        if (mainVentylator != null)
        {
            mainVentylator.parent = mainVentylatorParent;
        }

        foreach (GameObject obj in childEnable)
        {
            obj.SetActive(false);
        }
        interact.EnableObjectToDisable();
    }

    public void DisableWaitCoroutine()
    {
        if(waitForDisableChild!=null)
        {
            StopCoroutine(waitForDisableChild);
            waitForDisableChild = null;
        }
    }
}
