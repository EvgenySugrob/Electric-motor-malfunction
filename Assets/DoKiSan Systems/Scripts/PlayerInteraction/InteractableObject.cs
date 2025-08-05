using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("ID object")]
    [SerializeField] private string objectID;
    [SerializeField] public bool IsHighlightedByScenario = false;

    [Header("Trigger Steps")]
    [SerializeField] private string triggerName;

    [SerializeField] private MotorController motorController;
    [SerializeField] private GameObject[] prerequisiteObjects; // Предшественники как GameObject
    [SerializeField] private GameObject[] nextRequisiteObjects;
    [SerializeField] private GameObject[] objectsToDisable; // Объекты для отключения/включения
    [SerializeField] private bool isAnimated = false; // Использовать анимацию
    [SerializeField] private bool isInitiallyActive = true; // Состояние по умолчанию
    [SerializeField] private Outline outline;
    [SerializeField] private Color stepColorOutline;
    [SerializeField] private Color defaultColorOutline;
    private Animator animator;
    [SerializeField] private bool isDisassembled = false; // Состояние разборки
    [SerializeField] private PowerSwitch powerSwitch;

    [Header("Animation Setting")]
    [SerializeField] AnimationDisassembly animationDisassembly;

    [Header("Outline")]
    [SerializeField] List<Outline> childsOutline;

    [Header("LogText")]
    [SerializeField] string firstTextToLog;
    [SerializeField] string secondTextToLog;


    public GameObject[] PrerequisiteObjects
    {
        get => prerequisiteObjects;
        set => prerequisiteObjects = value;
    }

    public GameObject[] ObjectsToDisable
    {
        get => objectsToDisable;
        set => objectsToDisable = value;
    }

    public bool IsAnimated
    {
        get => isAnimated;
        set => isAnimated = value;
    }

    public bool IsDisassembled
    {
        get => isDisassembled;
        set => isDisassembled = value;
    }

    private void Awake()
    {
        HighlightRegistry.Register(objectID, this);
    }

    private void Start()
    {
        animationDisassembly = GetComponent<AnimationDisassembly>();
        outline= GetComponent<Outline>();
        if(outline!=null)
            defaultColorOutline = outline.OutlineColor;
    }

    public void OnInteract()
    {
        //if (motorController.GetIsMotorActive())
        if(powerSwitch.GetIsOn())
            return;

        if (isDisassembled && CanAssemble())
        {
            Assemble();
        }
        else if (!isDisassembled && CanDisassemble())
        {
            Disassemble();
        }
    }

    public void OnHoverEnter()
    {
        //if ((isDisassembled && CanAssemble()) || (!isDisassembled && CanDisassemble()))
        //{
        //    //EnableHighlight();
        //}
        if(IsHighlightedByScenario)
        {
            if(outline!=null)
            {
                outline.OutlineColor = defaultColorOutline;
            }
            else
            {
                foreach (Outline childOutline in childsOutline)
                {
                    childOutline.OutlineColor = defaultColorOutline;
                }
            }
        }

        if (outline != null)
        {
            if ((isDisassembled && CanAssemble()) || (!isDisassembled && CanDisassemble()))
            {
                outline.enabled = true;
            }
        }
        else
        {
            if ((isDisassembled && CanAssemble()) || (!isDisassembled && CanDisassemble()))
            {
                foreach (Outline childOutline in childsOutline)
                {
                    childOutline.enabled = true;
                }
            }
        }
    }

    public void OnHoverExit()
    {
        //DisableHighlight();

        if(IsHighlightedByScenario)
        {
            if(outline!=null)
            {
                outline.OutlineColor = stepColorOutline;
            }
            else
            {
                foreach (Outline childOutline in childsOutline)
                {
                    childOutline.OutlineColor = stepColorOutline;
                }
            }
            
            return;
        }

        if (outline != null)
        {
            outline.enabled = false;
        }
        else
        {
            foreach (Outline childOutline in childsOutline)
            {
                childOutline.enabled = false;
            }
        }
    }

    public void EnableHighlight()
    {
        if (outline != null)
        {   
            if(IsHighlightedByScenario)
            {
                outline.OutlineColor = stepColorOutline;
            }
            outline.enabled = true;
        }
        else if (childsOutline != null)
        {
            foreach (var o in childsOutline)
            {
                if (o != null)
                {
                    if(IsHighlightedByScenario)
                        o.OutlineColor = stepColorOutline;

                    o.enabled = true;
                }
            } 
        }
    }

    public void DisableHighlight()
    {
        if (outline != null)
        {
            outline.enabled = false;
            outline.OutlineColor = defaultColorOutline;
        }
        else if (childsOutline != null)
        {
            foreach (var o in childsOutline)
            {
                if (o != null)
                {
                    o.enabled = false;
                    if (IsHighlightedByScenario)
                        o.OutlineColor = defaultColorOutline;
                }
            }                
        }
    }
    public void SetHighlight(bool state)
    {
        IsHighlightedByScenario = state;

        if (state)
        {
            EnableHighlight();
        }
        else
        {
            DisableHighlight();
        }
            
        //if (outline != null)
        //{
        //    if (state)
        //    {
        //        outline.OutlineColor = stepColorOutline;
        //        EnableHighlight();
        //    }
        //    else
        //    {
        //        outline.OutlineColor = defaultColorOutline;
        //        DisableHighlight();
        //    }
        //}
    }

    private bool CanDisassemble()
    {
        if (prerequisiteObjects != null)
        {
            foreach (var prerequisite in prerequisiteObjects)
            {
                if (prerequisite != null)
                {
                    InteractableObject prereq = prerequisite.GetComponent<InteractableObject>();
                    if (prereq != null && !prereq.IsDisassembled)
                        return false; // Блокируем разборку, если предшественник не снят
                }
            }
        }
        return true;
    }

    private void CheckedForNextStep()
    {
        InstructionManager.Instance.OnEventTriggered(triggerName,0);
        LoggingUserActions.Instance.AddUserActionInLog(firstTextToLog);
        IsHighlightedByScenario = false;
    }

    public bool CanAssemble()
    {
        if (nextRequisiteObjects == null || nextRequisiteObjects.Length == 0)
        {
            return true;
        }

        foreach (var next in nextRequisiteObjects)
        {
            if (next != null)
            {
                InteractableObject nextObj = next.GetComponent<InteractableObject>();
                if (nextObj != null && nextObj.IsDisassembled)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Disassemble()
    {
        if (animationDisassembly!=null)
        {
            animationDisassembly.DisassemblyAnimationStart();
        }
        else
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null) 
                    obj.SetActive(false);
            }
        }
        isDisassembled = true;
        AssembleManager.Instance.AddDisassembledObject(this); // Добавляем в список разобранных

        CheckedForNextStep();
    }

    private void Assemble()
    {
        if (animationDisassembly!=null)
        {
            animationDisassembly.AssemblyAnimationStart();
        }
        else
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null) 
                    obj.SetActive(true);
            }
        }
        isDisassembled = false;
        AssembleManager.Instance.RemoveDisassembledObjecct(this);

        LoggingUserActions.Instance.AddUserActionInLog(secondTextToLog);
    }

    public void EnableObjectToDisable()
    {
        foreach(GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        HighlightRegistry.Unregister(objectID);
    }

    public string GetObjectID()
    {
        return objectID;
    }
}
