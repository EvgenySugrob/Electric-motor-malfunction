using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementManager : MonoBehaviour
{
    public FaultScenario activeScenario;

    private string pointA;
    private string pointB;

    public void SetScenario(FaultScenario scenario)
    {
        activeScenario = scenario;
        activeScenario.InitializeScenario();
    }

    public void SelectPoint(string pointId)
    {
        if (string.IsNullOrEmpty(pointA))
        {
            pointA = pointId;
            Debug.Log($"������� ������ �����: {pointA}");
        }
        else
        {
            pointB = pointId;
            Debug.Log($"������� ������ �����: {pointB}");

            PerformMeasurement();
        }
    }

    private void PerformMeasurement()
    {
        if (activeScenario == null)
        {
            Debug.LogWarning("��� ��������� ��������!");
            return;
        }

        string result = activeScenario.GetMeasurementResult(pointA, pointB);
        Debug.Log($"��������� ��������� ����� {pointA} � {pointB}: {result}");

        // ������� �����
        pointA = null;
        pointB = null;

        // TODO: ���������� ��������� �� ������ �������
    }
}
