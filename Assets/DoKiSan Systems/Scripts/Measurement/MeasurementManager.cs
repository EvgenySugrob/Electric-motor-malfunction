using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasurementManager : MonoBehaviour
{
    [Header("MainSetting")]
    [SerializeField] private FaultScenario activeScenario;
    [SerializeField] private TMP_Text multimeterDisplayText;

    [Header("Multimeter")]
    [SerializeField] Multimeter multimeter;

    [Header("LogText")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject prefabTextSpawn;
    private TMP_Text prefabText;

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
        //if(string.IsNullOrEmpty(pointA) && string.IsNullOrEmpty(pointB))
        //{
        //    PerformMeasurement();
        //}
    }

    private void PerformMeasurement()
    {
        if (activeScenario == null)
        {
            Debug.LogWarning("��� ��������� ��������!");
            return;
        }

        string result = activeScenario.GetMeasurementResult(pointA, pointB);
        multimeterDisplayText.text = result;

        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        GameObject spawnLogText = Instantiate(prefabTextSpawn,spawnPoint);
        spawnLogText.transform.SetAsFirstSibling();
        prefabText = spawnLogText.GetComponent<TMP_Text>();
        prefabText.text = $"{pointA} - {pointB}: {result} Om - {currentTime}";
        spawnLogText.SetActive(true);

        multimeter.ProbesBack();

        Debug.Log($"��������� ��������� ����� {pointA} � {pointB}: {result}");

        pointA = null;
        pointB = null;
    }
}
