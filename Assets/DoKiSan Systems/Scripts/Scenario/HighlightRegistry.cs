using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighlightRegistry 
{
    private static Dictionary<string, IInteractable> _registry = new();

    public static void Register(string id, IInteractable obj)
    {
        if (!_registry.ContainsKey(id))
            _registry.Add(id, obj);
    }

    public static void Unregister(string id)
    {
        _registry.Remove(id);
    }

    public static IInteractable Get(string id)
    {
        _registry.TryGetValue(id, out var obj);
        return obj;
    }
}
