using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Замыкание на корпус")]
public class WindingToCaseShort : FaultScenario, IMotorFaultTypeProvider
{
    private string brokenPoint;
    [Header("Multimeter")]
    [SerializeField] private float minValue = 50.08f;
    [SerializeField] private float maxValue = 51.27f;
    [SerializeField] private float minDefect = 9.51f;
    [SerializeField] private float maxDefect = 11.95f;
    [Header("Meagaommeter")]
    [SerializeField] private float breakAngle = 15f;
    [SerializeField] private float normalAngle = 68f;

    public override void InitializeScenario()
    {
        var point = new List<string> { "U1", "U2", "V1", "V2", "W1", "W2" };
        brokenPoint = point[Random.Range(0, point.Count)];

        Debug.Log($"[Fault Init] Замыкание: {brokenPoint} и GR");
    }

    private bool IsBroken(string a, string b)
    {
        return (a == brokenPoint && b == "Ground") ||
               (b == brokenPoint && a == "Ground");
    }

    //public override MeasurementResult GetMeasurementResult(string pointA, string pointB)
    //{
    //    bool isBroken = (pointA == brokenPoint && pointB == "Ground") ||
    //        (pointB == brokenPoint && pointA == "Ground");

    //    if(isBroken)
    //    {
    //        return new MeasurementResult(GetDefectValue().ToString(), breakAngle);
    //    }
    //    else
    //    {
    //        float normalValue = GetNormalValue();
    //        return new MeasurementResult(normalValue.ToString(), normalAngle);
    //    }
    //}

    public override MeasurementResult GetMultimeterResult(string a, string b)
    {
        if (IsBroken(a, b))
        {
            return new MeasurementResult(GetDefectValue().ToString(), 0f);
        }
        else
        {
            float normalValue = GetNormalValue();
            return new MeasurementResult(normalValue.ToString(), 0);
        }
    }

    public override MeasurementResult GetMegaommeterResult(string a, string b)
    {
        if (IsBroken(a,b))
        {
            return new MeasurementResult("0", breakAngle);
        }
        else
        {
            return new MeasurementResult("0", normalAngle);
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
