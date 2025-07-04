using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosticTool : MonoBehaviour
{
    [SerializeField] private DiagnosticToolType toolType;
    private string diagnosticResult;

    public void Initialize(string result)
    {
        Debug.Log(gameObject.name);
        diagnosticResult = result;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void PerformDiagnostic() //взаимодействие пользователя с инструментом
    {
        if(toolType == DiagnosticToolType.ManualCheck)
        {
            Debug.Log("Визуальный осмотр");
        }
        else
        {
            Debug.Log($"Диагностика с помощью {toolType}: {diagnosticResult}");
        }
        //UIManager.Instance.ShowDiagnosticResult(diagnosticResult);
    }

    public DiagnosticToolType ToolType => toolType;
}
