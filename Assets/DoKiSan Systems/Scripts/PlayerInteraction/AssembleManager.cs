using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssembleManager : MonoBehaviour
{
    [SerializeField] private List<InteractableObject> disassembledList = new List<InteractableObject>();
    private static AssembleManager instance;

    public static AssembleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AssembleManager>();
            }
            return instance;
        }
    }

    public void AddDisassembledObject(InteractableObject obj)
    {
        if (!disassembledList.Contains(obj))
        {
            disassembledList.Add(obj);
        }
    }

    public void RemoveDisassembledObjecct(InteractableObject obj)
    {
        if(disassembledList.Contains(obj))
        {
            disassembledList.Remove(obj);
        }
    }

    public void AutoAssemble()
    {
        if (disassembledList.Count == 0) return;

        // Идем с конца списка (обратный порядок)
        for (int i = disassembledList.Count - 1; i >= 0; i--)
        {
            InteractableObject obj = disassembledList[i];
            if (obj != null && obj.CanAssemble())
            {
                obj.OnInteract();
                //disassembledList.RemoveAt(i); // Удаляем из списка после сборки
            }
        }
    }

    public bool IsListFreeOfObject()
    {
        bool isFree = false;

        if(disassembledList.Count>0)
            isFree = false;
        else
            isFree = true;

        return isFree;
    }
}
