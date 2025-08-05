using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerSwitch : MonoBehaviour, IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] public bool IsHighlightedByScenario = false;
    [SerializeField] float timeWait=3f;

    [Header("Main Setting")]
    [SerializeField] MotorController motorController;
    [SerializeField] AssembleManager assembleManager;
    [SerializeField] Outline outline;
    [SerializeField] Color stepColorOutline;
    [SerializeField] float durationAnimation = 0.5f;
    [SerializeField] Vector3 angleRotate = new Vector3(0,0,0);
    [SerializeField]private bool isOn = false;
    [SerializeField]private Color defaultColorOutline;

    [Header("LogText")]
    [SerializeField] string firstTextToLog = "Включение электродвигателя.";
    [SerializeField] string secondTextToLog = "Выключение электродвигателя.";

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        defaultColorOutline = outline.OutlineColor;
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

        if(isOn)
        {
            InstructionManager.Instance.OnEventTriggered("SwitchOn",timeWait);
            LoggingUserActions.Instance.AddUserActionInLog(firstTextToLog);
        }
        else
        {
            InstructionManager.Instance.OnEventTriggered("SwitchOff",0);
            LoggingUserActions.Instance.AddUserActionInLog(secondTextToLog);
        }

        if (!motorController.GetIsStopedNow())
        {
            motorController.ActiveMotorButtonClick();
        }
    }

    public bool GetIsOn()
    {
        return isOn;
    }

    public string GetObjectID()
    {
        return objectID;
    }

    public void SetHighlight(bool state)
    {
        if (this == null) return; // защита от уничтоженного объекта
        if (gameObject == null) return;

        IsHighlightedByScenario = state;

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
