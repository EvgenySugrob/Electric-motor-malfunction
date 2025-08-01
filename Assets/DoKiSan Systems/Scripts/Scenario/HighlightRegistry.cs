using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighlightRegistry 
{
    private static Dictionary<string, IInteractable> _registry = new();

    public static void Register(string id, IInteractable obj)
    {
        if (string.IsNullOrEmpty(id))
            return;

        _registry[id] = obj;
        Debug.Log("REGISTR - " + id);
        //if (!_registry.ContainsKey(id))
        //    _registry.Add(id, obj);
    }

    public static void Unregister(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) 
            return;

        _registry.Remove(id);
    }

    public static IInteractable Get(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) 
            return null;

        _registry.TryGetValue(id, out var obj);
        return obj;
    }

    public static void Clear()
    {
        _registry.Clear();
    }
}
