using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/Выход из строя колец или щеток")]
public class SlipRingOrBrushFailureFault : FaultScenario, IMotorFaultTypeProvider
{
    [Header("Multimeter")]
    [SerializeField] private float minValue = 50.08f;
    [SerializeField] private float maxValue = 51.27f;

    [Header("Megaommeter")]
    [SerializeField] private float normalAngle = 68f;

    public override void InitializeScenario()
    {
        Debug.Log("[Fault Init] Замена моделей токосъемных колец и щеток");
        ReplaceModels();
    }

    public override void ReplaceModels()
    {
        FaultModelReplacer replacer = GameObject.FindObjectOfType<FaultModelReplacer>();
        
        if(replacer!=null)
        {
            replacer.OnlyRing();
        }
    }

    public override MeasurementResult GetMultimeterResult(string a, string b)
    {
        return new MeasurementResult(GetNormalValue().ToString(), 0f); // просто пример
    }

    public override MeasurementResult GetMegaommeterResult(string a, string b)
    {
        return new MeasurementResult("0", normalAngle); // просто пример
    }

    public MotorFaultType GetMotorFaultType() => MotorFaultType.SlipRingOrBrushFailure;

    private float GetNormalValue()
    {
        float value = Random.Range(minValue, maxValue);
        return Mathf.Round(value * 100f) / 100f;
    }
}
