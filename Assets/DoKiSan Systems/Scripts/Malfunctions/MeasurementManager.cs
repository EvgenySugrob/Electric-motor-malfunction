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
            Debug.Log($"Выбрана первая точка: {pointA}");
        }
        else
        {
            pointB = pointId;
            Debug.Log($"Выбрана вторая точка: {pointB}");

            PerformMeasurement();
        }
    }

    private void PerformMeasurement()
    {
        if (activeScenario == null)
        {
            Debug.LogWarning("Нет активного сценария!");
            return;
        }

        string result = activeScenario.GetMeasurementResult(pointA, pointB);
        Debug.Log($"Результат измерения между {pointA} и {pointB}: {result}");

        // Сбросим точки
        pointA = null;
        pointB = null;

        // TODO: отобразить результат на экране прибора
    }
}
