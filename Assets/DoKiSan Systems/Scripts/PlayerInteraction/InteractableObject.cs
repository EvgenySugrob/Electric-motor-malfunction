using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private MotorController motorController;
    [SerializeField] private GameObject[] prerequisiteObjects; // Предшественники как GameObject
    [SerializeField] private GameObject[] nextRequisiteObjects;
    [SerializeField] private GameObject[] objectsToDisable; // Объекты для отключения/включения
    [SerializeField] private bool isAnimated = false; // Использовать анимацию
    [SerializeField] private bool isInitiallyActive = true; // Состояние по умолчанию
    [SerializeField] private Outline outline;
    private Color originalColor;
    private Animator animator;
    [SerializeField] private bool isDisassembled = false; // Состояние разборки
    [SerializeField] private PowerSwitch powerSwitch;

    [Header("Animation Setting")]
    [SerializeField] AnimationDisassembly animationDisassembly;

    [Header("Outline")]
    [SerializeField] List<Outline> childsOutline;

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


    private void Start()
    {
        animationDisassembly = GetComponent<AnimationDisassembly>();
        outline= GetComponent<Outline>();
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
        if(outline != null)
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
        Debug.Log(name+ " HoverExit");

        if(outline != null)
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
    }

    public void EnableObjectToDisable()
    {
        foreach(GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
