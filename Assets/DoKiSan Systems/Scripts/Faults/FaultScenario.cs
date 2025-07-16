using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FaultScenario : ScriptableObject
{
    [Header("��� ��������")]
    public string scenarioName;

    [Header("�����, � ������� ������������ ���������")]
    public List<string> involvedPoints = new(); // ��������: "U2", "V2", "W2"

    public abstract void InitializeScenario();
    public abstract MeasurementResult GetMultimeterResult(string a, string b);
    public abstract MeasurementResult GetMegaommeterResult(string a, string b);

    public virtual void ReplaceModels()
    {

    }
}
