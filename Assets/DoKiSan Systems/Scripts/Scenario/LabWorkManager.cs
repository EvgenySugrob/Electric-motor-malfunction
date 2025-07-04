using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LabWorkManager
{
    public static string CurrentLabWork { get; private set; }

    public static void SetLabWork(string labWork)
    {
        CurrentLabWork = labWork;
        Debug.Log($"Lab wok set to: {labWork}");
    }

    public static bool IsLabWorkActive(string labWork)
    {
        return CurrentLabWork == labWork;
    }
}
