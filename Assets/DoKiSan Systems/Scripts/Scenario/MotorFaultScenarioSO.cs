using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Motor Fault Scenario/New Scenario")]
public class MotorFaultScenarioSO : ScriptableObject
{
    public MotorFaultType faultType;
    [TextArea] public string description;
    public List<string> symptoms;
    public List<DiagnosticToolType> diagnosticTools;
    [TextArea] public string diagnosticResult;
    [TextArea] public string fixAction;

    public List<InstructionStep> instructionSteps;

    public FaultScenario measurementLogic;
}
