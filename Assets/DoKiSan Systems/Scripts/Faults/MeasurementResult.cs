using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeasurementResult
{
    public string displayText;
    public float needleAngle;
    public bool isValid;

    public MeasurementResult(string text, float angle, bool valid = true)
    {
        displayText = text; 
        needleAngle = angle; 
        isValid = valid;
    }
}
