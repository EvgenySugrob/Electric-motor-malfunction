using DG.Tweening;
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
    [SerializeField] Megaommetr megaommetr;
    [SerializeField] Transform megaommeterNeedle;
    [SerializeField] float needleMoveDuration = 0.25f;

    [Header("LogText")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject prefabTextSpawn;
    [SerializeField] string textToMoveProbe;
    private TMP_Text prefabText;

    [Header("MouseCursorHandler")]
    [SerializeField] MouseCursorHandler mouseCursorHandler;

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

        if (megaommeterNeedle != null)
        {
            megaommeterNeedle
                .DOLocalRotate(new Vector3(0, -89.5f,0), needleMoveDuration)
                .SetEase(Ease.OutQuad)
                .Play();
        }
    }

    private void TryMeasure()
    {
        if(!string.IsNullOrEmpty(pointA) && !string.IsNullOrEmpty(pointB))
        {
            MeasurementResult result;

            if(mouseCursorHandler.GetCurrentInstrument().GetComponent<Multimeter>()==multimeter)
            {
                result = activeScenario.GetMultimeterResult(pointA, pointB);
            }
            else
            {
                result = activeScenario.GetMegaommeterResult(pointA, pointB);
            }

            multimeterDisplayText.text = result.displayText;

            // Поворачиваем стрелку мегаомметра, если она есть
            if (megaommeterNeedle != null && mouseCursorHandler.GetCurrentInstrument().GetComponent<Megaommetr>()==megaommetr)
            {
                megaommeterNeedle
                    .DOLocalRotate(new Vector3(0, -result.needleAngle, 0), needleMoveDuration)
                    .SetEase(Ease.OutQuad)
                    .Play();
            }

            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            GameObject spawnLogText = Instantiate(prefabTextSpawn, spawnPoint);
            spawnLogText.transform.SetAsFirstSibling();

            prefabText = spawnLogText.GetComponent<TMP_Text>();
            prefabText.text = $"{pointA} - {pointB}: {result.displayText} Om - {currentTime}";

            spawnLogText.SetActive(true);
        }
    }

    public void EnableTextMoveProbe()
    {
        multimeterDisplayText.text = textToMoveProbe + "\t";
    }
}
