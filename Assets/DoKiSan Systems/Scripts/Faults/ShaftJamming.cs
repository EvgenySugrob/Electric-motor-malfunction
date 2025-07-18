using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Заклинивание вала")]
public class ShaftJamming : FaultScenario, IMotorFaultTypeProvider
{
    [Header("Multimeter")]
    [SerializeField] private float minValue = 50.08f;
    [SerializeField] private float maxValue = 51.27f;

    [Header("Megaommeter")]
    [SerializeField] private float normalAngle = 68f;

    public override void InitializeScenario()
    {
        Debug.Log($"[Fault Init] Заклинивание вала");
    }

    public override MeasurementResult GetMegaommeterResult(string a, string b)
    {
        return new MeasurementResult("0", normalAngle);
    }

    public override MeasurementResult GetMultimeterResult(string a, string b)
    {
        return new MeasurementResult(GetNormalValue().ToString(), 0f);
    }

    public MotorFaultType GetMotorFaultType() => MotorFaultType.ShaftJamming;

    private float GetNormalValue()
    {
        float value = Random.Range(minValue, maxValue);
        return Mathf.Round(value * 100f) / 100f;
    }

}
