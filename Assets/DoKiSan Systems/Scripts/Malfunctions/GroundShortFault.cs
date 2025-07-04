using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Замыкание на корпус")]
public class GroundShortFault : FaultScenario
{
    private string shortedPoint;

    public override void InitializeScenario()
    {
        // Выбираем одну из фаз, у которой "замыкание на корпус"
        var candidates = new List<string> { "U1", "V1", "W1", "U2", "V2", "W2" };
        shortedPoint = candidates[Random.Range(0, candidates.Count)];
    }

    public override string GetMeasurementResult(string pointA, string pointB)
    {
        if ((pointA == shortedPoint && pointB == "GND") || (pointB == shortedPoint && pointA == "GND"))
            return "Короткое на корпус";

        return "МегаОм"; // Нормальная изоляция
    }
}
