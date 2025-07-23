using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InstructionStep
{
    public string stepText;
    public List<string> objectIDsToHighlight;
    public List<string> requiredEvents;
}
