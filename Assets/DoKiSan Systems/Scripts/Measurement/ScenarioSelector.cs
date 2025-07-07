using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSelector : MonoBehaviour
{
    public FaultScenario selectedScenario;

    private void Start()
    {
        if (selectedScenario != null)
        {
            selectedScenario.InitializeScenario();
            FindObjectOfType<MeasurementManager>().SetScenario(selectedScenario);
        }
    }
}
