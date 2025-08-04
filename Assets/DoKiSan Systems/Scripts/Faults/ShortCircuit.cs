using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Замыкание между обмотками")]
public class ShortCircuit : FaultScenario, IMotorFaultTypeProvider
{
    private (string, string) brokenPair;

    [Header("Multimeter")]
    [SerializeField] private float minValue = 50.05f;
    [SerializeField] private float maxValue = 51.30f;
    [SerializeField] private float minDefect = 9.51f;
    [SerializeField] private float maxDefect = 11.95f;

    [Header("Megaommeter")]
    [SerializeField] private float breakAngle = 15;
    [SerializeField] private float normalAngle = 68;

    public override void InitializeScenario()
    {
        var candidates = new List<(string, string)>
        {
            ("U1", "V1"),
            ("V1", "W1"),
            ("U1", "W1")
        };

        int index = Random.Range(0, candidates.Count);
        brokenPair = candidates[index];

        ReplaceModels();

        Debug.Log($"[Fault Init] Замыкание между: {brokenPair.Item1} и {brokenPair.Item2}");
    }

    public override void ReplaceModels()
    {
        FaultModelReplacer replacer = GameObject.FindObjectOfType<FaultModelReplacer>();

        if (replacer != null)
        {
            replacer.ProgarRandom();
        }
    }

    private bool IsBroken(string a, string b)
    {
        return (a == brokenPair.Item1 && b == brokenPair.Item2) ||
            (a == brokenPair.Item2 && b == brokenPair.Item1);
    }

    public override MeasurementResult GetMegaommeterResult(string a, string b)
    {
        if (IsBroken(a, b))
        {
            return new MeasurementResult("0", breakAngle); 
        }
        else
        {
            return new MeasurementResult("0", normalAngle); 
        }
    }

    public override MeasurementResult GetMultimeterResult(string a, string b)
    {
        if (IsBroken(a, b))
        {
            InstructionManager.Instance.OnEventTriggered("ShortCircuit", 0f);
            return new MeasurementResult(GetDefectValue().ToString(), 0f);
        }
        else
        {
            float normalValue = GetNormalValue();
            return new MeasurementResult(normalValue.ToString(), 0);
        }
    }

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

    public MotorFaultType GetMotorFaultType() => MotorFaultType.WindingShortCircuit;
}
