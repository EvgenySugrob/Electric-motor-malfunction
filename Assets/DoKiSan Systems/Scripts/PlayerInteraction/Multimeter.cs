using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multimeter : MonoBehaviour,IInteractable
{
    [Header("AnimationInHand")]
    [SerializeField] GameObject toolForHide;
    [SerializeField] List<Transform> pointsInHand;
    [SerializeField] float durationBetweenPoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [Header("MultimetrProbes")]
    [SerializeField] List<MultimeterProbes> multimeterProbes;

    [Header("OutlineControl")]
    [SerializeField] Outline caseOutline;

    [Header("PlayerControl")]
    [SerializeField] CameraController cameraController;
    [SerializeField] MouseCursorHandler cursorHandler;

    [Header("MultimetrState")]
    [SerializeField] private bool isInHand = false;
    [SerializeField] MultimeterSlotBackPosition slotBack;
    private bool isSecondPoint = false;

    [Header("WheelMode")]
    [SerializeField] MultimetrModeWheel wheelMode;

    [Header("Overlay")]
    [SerializeField] GameObject cameraOverlay;

    private BoxCollider mainCollider;

    private void Start()
    {
        mainCollider = GetComponent<BoxCollider>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void OnHoverEnter()
    {
        caseOutline.enabled = true;
    }

    public void OnHoverExit()
    {
        caseOutline.enabled = false;
    }

    public void OnInteract()
    {
        cursorHandler.IsHandSlotBusy = true;

        TakeToolInHand();
    }

    private void TakeToolInHand()
    {
        StartCoroutine(TakeTool());
    }

    private void PlayerControlDisable(bool isState)
    {
        cameraController.enabled = isState;
        cursorHandler.enabled = isState;
    }

    private IEnumerator TakeTool()
    {
        if(!isInHand)
        {
            cameraOverlay.SetActive(true);

            if(cursorHandler.GetCurrentInstrument()!=transform.gameObject)
            {
                cursorHandler.SetCurrentInstrument(transform.gameObject);
            }
            PlayerControlDisable(false);

            if (mainCollider != null)
                mainCollider.enabled = false;

            toolForHide.SetActive(false);
            isInHand = true;

            transform.position = pointsInHand[0].position;
            transform.eulerAngles = pointsInHand[0].eulerAngles;
            transform.parent = cameraController.transform;

            foreach(MultimeterProbes probe in multimeterProbes)
            {
                probe.gameObject.SetActive(true);
            }

            yield return ToolMoveUp();

            PlayerControlDisable(true);
            mainCollider.enabled = true;

            wheelMode.WheelColliderState(true);
            slotBack.gameObject.SetActive(true);
        }
        else
        {
            ToolAfterChoiseMode();
        }
    }

    private YieldInstruction ToolMoveUp()
    {
        toolForHide.SetActive(true);

        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[1].position, durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[1].eulerAngles, durationBetweenPoint/2))
            .Append(transform.DOMove(pointsInHand[2].position,durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[2].eulerAngles, durationBetweenPoint/2))
            .Play()
            .WaitForCompletion();
    }

    public void ToolAfterChoiseMode()
    {
        PlayerControlDisable(false);

        if (!isSecondPoint)
        {
            StartCoroutine(TweenToolToSecondPoint());
        }
        else
        {
            StartCoroutine(TweenToShowChoiseMode());
        }
        isSecondPoint = !isSecondPoint;
    }

    private IEnumerator TweenToolToSecondPoint()
    {
        yield return ToSecondPoint();

        PlayerControlDisable(true);
    }

    private IEnumerator TweenToShowChoiseMode()
    {
        yield return ToChoiceMode();

        PlayerControlDisable(true);
    }

    private YieldInstruction ToZeroPoint()
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[0].position, durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[0].eulerAngles, durationBetweenPoint / 2))
            .Play()
            .WaitForCompletion();
    }

    private YieldInstruction ToSecondPoint()
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[1].position, durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[1].eulerAngles, durationBetweenPoint / 2))
            .Play()
            .WaitForCompletion();
    }

    private YieldInstruction ToChoiceMode()
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[2].position, durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[2].eulerAngles, durationBetweenPoint / 2))
            .Play()
            .WaitForCompletion();
    }

    public void PutOnTable()
    {
        StartCoroutine(OnTable());
    }


    private IEnumerator OnTable()
    {
        PlayerControlDisable(false);

        wheelMode.WheelColliderState(false);
        wheelMode.DisableAndWheelOnStart();

        cursorHandler.SetCurrentInstrument(null);

        yield return ToZeroPoint();

        foreach (MultimeterProbes probe in multimeterProbes)
        {
            probe.gameObject.SetActive(false);
        }

        cameraOverlay.SetActive(false);

        isInHand = false;
        isSecondPoint = false;
        transform.parent = null;
        transform.position = startPosition;
        transform.rotation = startRotation;

        slotBack.OutlineForceDisable();

        PlayerControlDisable(true);
    }
}
