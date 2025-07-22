using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public static HighlightController Instance { get; private set; }

    [SerializeField] private List<GameObject> currentlyHighlighted = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void ClearHighlight()
    {
        foreach (var go in currentlyHighlighted)
        {
            Outline outline = go.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        currentlyHighlighted.Clear();
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

    public void HighlightObjects(List<string> objectIDs)
    {
        ClearHighlight();

        var interactables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();

        foreach (var interactable in interactables)
        {
            if (objectIDs.Contains(interactable.GetObjectID()))
            {
                GameObject go = ((MonoBehaviour)interactable).gameObject;

                // Включаем подсветку
                Outline outline = go.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    currentlyHighlighted.Add(go);
                }
                else
                {
                    Debug.LogWarning($"Объект {go.name} не имеет Outline-компонента");
                }
            }
        }
    }

}
