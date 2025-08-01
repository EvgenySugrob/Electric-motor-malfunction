using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaommetr : MonoBehaviour, IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] string triggerEventName = "MegaommeterTake";
    [SerializeField] public bool IsHighlightedByScenario = false;

    [Header("AnimationInHand")]
    [SerializeField] GameObject toolForHide;
    [SerializeField] List<Transform> pointsInHand;
    [SerializeField] float durationBetweenPoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [Header("OutlineControl")]
    [SerializeField] Outline caseOutline;
    [SerializeField] Color stepColorOutline;
    private Color defaultColorOutline;

    [Header("Probes")]
    [SerializeField] List<MultimeterProbes> multimeterProbes;

    [Header("PlayerControl")]
    [SerializeField] CameraController cameraController;
    [SerializeField] MouseCursorHandler cursorHandler;

    [Header("MegaommetrState")]
    [SerializeField] private bool isInHand = false;
    [SerializeField] MegaommetrBackPosition megaommetrBackPosition;

    [Header("Overlay")]
    [SerializeField] GameObject cameraOverlay;

    private BoxCollider mainCollider;

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        mainCollider = GetComponent<BoxCollider>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        defaultColorOutline = caseOutline.OutlineColor;
    }

    public void OnHoverEnter()
    {
        if (IsHighlightedByScenario)
        {
            caseOutline.OutlineColor = defaultColorOutline;
            return;
        }
        caseOutline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (IsHighlightedByScenario)
        {
            caseOutline.OutlineColor = stepColorOutline;
            return;
        }

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

            if (cursorHandler.GetCurrentInstrument() != transform.gameObject)
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

            foreach (MultimeterProbes probe in multimeterProbes)
            {
                probe.gameObject.SetActive(true);
            }

            yield return ToolMoveUp();

            PlayerControlDisable(true);
            mainCollider.enabled = true;


            megaommetrBackPosition.gameObject.SetActive(true);
            InstructionManager.Instance.OnEventTriggered(triggerEventName, 0f);
        }
    }

    private YieldInstruction ToolMoveUp()
    {
        toolForHide.SetActive(true);

        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[1].position, durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[1].eulerAngles, durationBetweenPoint / 2))
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

        cursorHandler.SetCurrentInstrument(null);

        yield return ToZeroPoint();

        foreach (MultimeterProbes probe in multimeterProbes)
        {
            probe.gameObject.SetActive(false);
            probe.ProbeStartPosition();
            probe.ForceCLear();
        }

        cameraOverlay.SetActive(false);

        isInHand = false;
        transform.parent = null;
        transform.position = startPosition;
        transform.rotation = startRotation;

        megaommetrBackPosition.OutlineForceDisable();

        PlayerControlDisable(true);
    }

    private YieldInstruction ToZeroPoint()
    {
        return DOTween.Sequence()
            .Append(transform.DOMove(pointsInHand[0].position,durationBetweenPoint))
            .Join(transform.DORotate(pointsInHand[0].eulerAngles,durationBetweenPoint/2))
            .Play()
            .WaitForCompletion();
    }

    public void ForceProbeBack()
    {
        foreach(MultimeterProbes probe in multimeterProbes)
        {
            probe.ProbeBackAfterMeasurement();
            probe.ForceCLear();
        }
    }

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = state;
        if (caseOutline != null)
        {
            if (state)
            {
                caseOutline.OutlineColor = stepColorOutline;
                caseOutline.enabled = true;
            }
            else
            {
                caseOutline.OutlineColor = defaultColorOutline;
                caseOutline.enabled = false;
            }
        }
    }
}
