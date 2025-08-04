using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/ќбрыв обмотки статора")]
public class OpenWindingFault : FaultScenario, IMotorFaultTypeProvider
{
    private (string, string) brokenPair;
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
        var candidates = new List<(string, string)>
        {
            ("U2", "V2"),
            ("V2", "W2"),
            ("U2", "W2")
        };

        int index = Random.Range(0, candidates.Count);
        brokenPair = candidates[index];

        Debug.Log($"[Fault Init] ќбрыв между: {brokenPair.Item1} и {brokenPair.Item2}");
    }

    private bool IsBroken(string a, string b)
    {
        return (a == brokenPair.Item1 && b == brokenPair.Item2) ||
            (a == brokenPair.Item2 && b == brokenPair.Item1);
    }

    public override MeasurementResult GetMultimeterResult(string a, string b)
    {
        if(IsBroken(a,b))
        {
            InstructionManager.Instance.OnEventTriggered("OpenWindingFaultTrigger",0f);
            return new MeasurementResult(GetDefectValue().ToString(), 0f);
        }
        else
        {
            float normalValue = GetNormalValue();
            return new MeasurementResult(normalValue.ToString(),0);
        }
    }

    public override MeasurementResult GetMegaommeterResult(string a, string b)
    {
        if (IsBroken(a, b))
        {
            return new MeasurementResult("0", breakAngle); // стрелка уходит в край Ч замыкание
        }
        else
        {
            return new MeasurementResult("0", normalAngle); // условный "нормальный" угол
        }
    }

    public MotorFaultType GetMotorFaultType() => MotorFaultType.StatorWindingBreak;
    
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
