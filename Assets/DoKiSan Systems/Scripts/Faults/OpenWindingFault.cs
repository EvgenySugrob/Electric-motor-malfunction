using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Обрыв обмотки статора")]
public class OpenWindingFault : FaultScenario, IMotorFaultTypeProvider
{
    [SerializeField] private (string, string) brokenPair;

    public override void InitializeScenario()
    {
        var candidates = new List<(string, string)>
        {
            ("U2", "V2"),
            ("V2", "W2"),
            ("U2", "W2")
        };

        int index = Random.Range(0, candidates.Count);
        brokenPair = candidates[index];

        Debug.Log($"[Fault Init] Обрыв между: {brokenPair.Item1} и {brokenPair.Item2}");
    }

    public override string GetMeasurementResult(string a, string b)
    {
        bool isBroken =
            (a == brokenPair.Item1 && b == brokenPair.Item2) ||
            (a == brokenPair.Item2 && b == brokenPair.Item1);

        return isBroken ? "Break" : "50.12";
    }

    public MotorFaultType GetMotorFaultType() => MotorFaultType.StatorWindingBreak;
    
}
