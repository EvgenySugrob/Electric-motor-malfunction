using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectingLabWork : MonoBehaviour
{
    public void OnScenarioButtonClicked(int faultTypeIndex)
    {
        MotorFaultType faultType = (MotorFaultType)faultTypeIndex;
        ScenarioManager.Instance.SelectScenario(faultType);

        SceneManager.LoadScene("Electro_engine_LabWork");
    }

    public void OnExamButtonClicked()
    {
        ScenarioManager.Instance.StartExamMode();

        SceneManager.LoadScene("Electro_engine_LabWork");
    }
}
