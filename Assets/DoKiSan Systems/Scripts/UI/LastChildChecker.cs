using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastChildChecker : MonoBehaviour
{
    private void Start()
    {
        if(ScenarioManager.Instance.IsExam() == false)
        {
            gameObject.SetActive(false);
        }
    }
}
