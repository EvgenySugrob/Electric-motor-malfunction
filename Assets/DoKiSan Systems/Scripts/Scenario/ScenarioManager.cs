using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ScenarioManager;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance;
    //[SerializeField] private List<MotorFaultScenario> scenarios; // Список всех сценариев
    //[SerializeField] private MotorFaultScenario currentScenario;

    [SerializeField] private List<MotorFaultScenarioSO> scenarios;
    [SerializeField] private MotorFaultScenarioSO currentScenario;

    [SerializeField]private bool isExamMode;
    [SerializeField] private bool isSceneSetupPending;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Отписываемся от события
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Выбор сценария
    public void SelectScenario(MotorFaultType faultType, bool examMode = false)
    {
        currentScenario = scenarios.Find(s => s.faultType == faultType);
        isExamMode = examMode;
        isSceneSetupPending = true;

        Debug.Log(isSceneSetupPending);

        if (currentScenario == null)
        {
            Debug.LogError($"Сценарий для {faultType} не найден!");
        }
    }

    // Вызывается при загрузке новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Сцена загрузилась");
        if (isSceneSetupPending && scene.name == "Electro_engine_LabWork")
        {
            SetupScene();
            isSceneSetupPending = false;

            Debug.Log(isSceneSetupPending);
        }
    }

    // Настройка сцены
    private void SetupScene()
    {
        ConfigureMeasurementLogic();
        ConfigureMotorSymptoms();
        ConfigureDiagnosticTools();
        ConfigureFixAction();
        ConfigurableInstruction();


        ConfigureCrackSpawn(); //выкл на релиз
    }

    private void ConfigureCrackSpawn()
    {
        CrackSpawn crackSpawn = FindObjectOfType<CrackSpawn>();

        if(crackSpawn != null)
        {
            crackSpawn.CrackSpawnSetting();
        }
    }

    private void ConfigurableInstruction()
    {
        if (!isExamMode && currentScenario.instructionSteps != null && currentScenario.instructionSteps.Count > 0)
        {
            StartCoroutine(StartScenarioRoutine());
        }
    }
    private IEnumerator StartScenarioRoutine()
    {
        yield return new WaitForSeconds(1f); // дожидаемся всех Awake()
        InstructionManager.Instance.StartInstructions(currentScenario.instructionSteps);
    }

    private void ConfigureMeasurementLogic()
    {
        MeasurementManager mm = FindObjectOfType<MeasurementManager>();
        if (mm != null && currentScenario.measurementLogic != null)
        {
            mm.SetScenario(currentScenario.measurementLogic);
        }
        else
        {
            Debug.LogWarning("MeasurementManager или логика измерений не найдена.");
        }
    }

    private void ConfigureMotorSymptoms()
    {
        MotorController motor = FindObjectOfType<MotorController>();
        if (motor != null)
        {
            motor.SetSymptoms(currentScenario.symptoms);
        }
        else
        {
            Debug.LogWarning("MotorController не найден на сцене!");
        }
    }

    private void ConfigureDiagnosticTools()
    {
        DiagnosticTool[] tools = FindObjectsOfType<DiagnosticTool>();
        if (tools.Length == 0)
        {
            Debug.LogWarning("DiagnosticTool не найдены на сцене!");
        }

        foreach (DiagnosticTool tool in tools)
        {
            if (isExamMode)
            {
                tool.Initialize(currentScenario.diagnosticResult);
            }
            else
            {
                if (tool.ToolType == DiagnosticToolType.ManualCheck)
                {
                    tool.Initialize(currentScenario.diagnosticResult);
                }
                else if (currentScenario.diagnosticTools.Contains(tool.ToolType))
                {
                    tool.Initialize(currentScenario.diagnosticResult);
                }
                else
                {
                    tool.Deactivate();
                }
            }
        }
    }

    private void ConfigureFixAction()
    {
        //FixButton fixButton = FindObjectOfType<FixButton>();
        //if (fixButton != null)
        //{
        //    fixButton.SetFixAction(currentScenario.fixAction);
        //}
        //else
        //{
        //    Debug.LogWarning("FixButton не найден на сцене!");
        //}
    }

    //public MotorFaultScenario GetCurrentScenario()
    //{
    //    return currentScenario;
    //}

    public void StartExamMode(int value)
    {       
        //int randomIndex = Random.Range(0, scenarios.Count);
        currentScenario = scenarios[value];
        isExamMode = true;
        isSceneSetupPending = true;
    }

    public bool IsExam()
    {
        return isExamMode;
    }

    public MotorFaultScenarioSO GetScenario()
    {
        return currentScenario;
    }
}

