using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public static HighlightController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void UnhighlightByID(string objectID)
    {
        if (HighlightRegistry.Get(objectID) is { } obj)
        {
            obj.SetHighlight(false);
        }
    }

    public void HighlightByID(string objectID)
    {
        var obj = HighlightRegistry.Get(objectID);

        if(obj!=null)
        {
            obj.SetHighlight(true);
        }
        else
        {
            Debug.LogWarning($"HighlightController: объект с ID {objectID} не найден в реестре.");
        }

        //if (HighlightRegistry.Get(objectID) is { } obj)
        //{
        //    obj.SetHighlight(true);
        //}
    }
}
