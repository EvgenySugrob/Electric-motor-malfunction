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

    [Header("Megaommetr")]
    [SerializeField] private float d = 0f;

    [Header("LogText")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject prefabTextSpawn;
    [SerializeField] string textToMoveProbe;
    private TMP_Text prefabText;

    private string pointA;
    private string pointB;

    public void SetScenario(FaultScenario scenario)
    {
        activeScenario = scenario;
        activeScenario.InitializeScenario();
    }

    public void SetProbePoint(MultimeterProbes probe, string pointId)
    {
        if (probe.GetProbeId() == 0)
            pointA = pointId;
        else if(probe.GetProbeId()==1)
            pointB = pointId;

        TryMeasure();
    }

    public void ClearProbePoint(MultimeterProbes probe)
    {
        if (probe.GetProbeId() == 0)
            pointA = null;
        else if (probe.GetProbeId() == 1)
            pointB = null;

        multimeterDisplayText.text = textToMoveProbe;

        //if (megohmmeterNeedle != null)
        //{
        //    megohmmeterNeedle
        //        .DOLocalRotate(Vector3.zero, needleMoveDuration)
        //        .SetEase(Ease.OutQuad);
        //}
        //сброс показания на мультиметре
    }

    private void TryMeasure()
    {
        if(!string.IsNullOrEmpty(pointA) && !string.IsNullOrEmpty(pointB))
        {
            MeasurementResult result = activeScenario.GetMeasurementResult(pointA, pointB);

            multimeterDisplayText.text = result.displayText;

            // Поворачиваем стрелку мегаомметра, если она есть
            //if (megohmmeterNeedle != null)
            //{
            //    megohmmeterNeedle
            //        .DOLocalRotate(new Vector3(0, 0, -result.needleAngle), needleMoveDuration)
            //        .SetEase(Ease.OutQuad);
            //}


            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            GameObject spawnLogText = Instantiate(prefabTextSpawn, spawnPoint);
            spawnLogText.transform.SetAsFirstSibling();

            prefabText = spawnLogText.GetComponent<TMP_Text>();
            prefabText.text = $"{pointA} - {pointB}: {result} Om - {currentTime}";

            spawnLogText.SetActive(true);
        }
    }

    public void EnableTextMoveProbe()
    {
        multimeterDisplayText.text = textToMoveProbe + "\t";
    }
}
