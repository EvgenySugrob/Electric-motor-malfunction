using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Замыкание на корпус")]
public class WindingToCaseShort : FaultScenario, IMotorFaultTypeProvider
{
    private string brokenPoint;
    [SerializeField] private float minValue = 50.08f;
    [SerializeField] private float maxValue = 51.27f;
    [SerializeField] private float minDefect = 9.51f;
    [SerializeField] private float maxDefect = 11.95f;

    public override void InitializeScenario()
    {
        var point = new List<string> { "U1", "U2", "V1", "V2", "W1", "W2" };
        brokenPoint = point[Random.Range(0, point.Count)];
    }

    public override MeasurementResult GetMeasurementResult(string pointA, string pointB)
    {
        bool isBroken = (pointA == brokenPoint && pointB == "Ground") ||
            (pointB == brokenPoint && pointA == "Ground");

        if(isBroken)
        {
            return new MeasurementResult(GetDefectValue().ToString(), 270f);
        }
        else
        {
            float normalValue = GetNormalValue();
            float angle = 0f;
            return new MeasurementResult(normalValue.ToString(), angle);
        }
    }

    public MotorFaultType GetMotorFaultType() => MotorFaultType.WindingToCaseShort;

    private float GetNormalValue()
    {
        float value = Random.Range(minValue, maxValue);
        return Mathf.Round(value * 100f) / 100f;
    }

    private float GetDefectValue()
    {
        float value = Random.Range(minDefect, maxDefect);
        return Mathf.Round(value * 100f) / 100f;
    }
}
