using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaultModelReplacer : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDisable;
    [SerializeField] GameObject[] objectsToEnable;

    [Header("Brushes")]
    [SerializeField] GameObject[] normalBrushes;
    [SerializeField] GameObject[] defectBrushes;

    [Header("Ring")]
    [SerializeField] GameObject[] normalRing;
    [SerializeField] GameObject[] defectRing;

    [Header("Progar")]
    [SerializeField] GameObject[] progar;

    public void ApplyReplacement()
    {
        foreach (GameObject obj in objectsToDisable)
            if (obj != null) obj.SetActive(false);

        foreach (GameObject obj in objectsToEnable)
            if (obj != null) obj.SetActive(true);
    }

    public void OnlyBrushes()
    {
        foreach (GameObject obj in normalBrushes)
            if(obj != null) 
                obj.SetActive(false);

        foreach (GameObject obj in defectBrushes)
            if(obj != null)
                obj.SetActive(true);
    }

    public void OnlyRing()
    {
        foreach (GameObject obj in normalRing)
            if (obj != null)
                obj.SetActive(false);

        foreach (GameObject obj in defectRing)
            if (obj != null)
                obj.SetActive(true);
    }

    public void ProgarRandom()
    {
        int willBe = Random.Range(0, 2);

        Debug.Log($"WILLBE = {willBe}");

        if(willBe == 1)
        {
            int index = Random.Range(0, progar.Length);

            progar[index].SetActive(true);

            Debug.Log($"Progar active {progar[index].name}");
        }
    }
}
