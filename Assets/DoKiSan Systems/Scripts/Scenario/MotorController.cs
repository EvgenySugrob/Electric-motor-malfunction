using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MotorController : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem sparkEffect;
    [SerializeField] private Renderer motorRenderer;
    [SerializeField] private Animator animator;            // �������� ��� ��������/������
    [SerializeField] private Color defaultMotorColor;
    [SerializeField] private Color hotMotorColor;
    [SerializeField] private Material instanceMaterial;
    [SerializeField] private Color currentColor;

    [Header("DOTween Settings")]
    [SerializeField] private float overheatingDuration = 1f;
    [SerializeField] private float powerCoefVibration = 0.12f;
    [SerializeField] private float vibrationDurationInterval = 0.2f;

    [Header("Rotation Setting")]
    [SerializeField] private Transform val;
    [SerializeField] private float defaultValRotationDuration = 1f;
    [SerializeField] private float rotationSpeed = 3600f;
    private Vector3 rotationAxis = Vector3.left;
    private Tween valRotationTween;
    private float currentSpeedRotation;

    [Header("ShaftPlay")]
    [SerializeField] Transform shaftValPosition;
    [SerializeField] Transform onlyVal;
    [SerializeField] Transform ventilytor;
    [SerializeField] Transform ventilytorMain;

    [Header("Jerking Setting")]
    [SerializeField] private float initialDelay = 2f;      
    [SerializeField] private float jerkInterval = 4f;      
    [SerializeField] private float jerkPauseDuration = 2f;

    [Header("Intermittent Settings")]
    [SerializeField] private float intermittentDelay = 1f;  
    [SerializeField] private float intermittentCycle = 3f;  
    [SerializeField] private float intermittentSlowFactor = 0.5f;
    [SerializeField] private float decelerationDurationFactor = 4f;

    [Header("Sioped Setting")]
    [SerializeField] float stopedDelay = 2f;
    private bool isStopedTrouble = false;
    private bool isStopedNow = false;

    [Header("State")]
    private bool isMotorActive = false;
    private bool isRunning = false;                        
    private bool isFixed = false;
    [SerializeField]private bool isLowPowerTrouble = false;
    private Sequence jerkSequence;                                               
    private Tween vibrationTween;
    private Tween overheatingTween;
    private Sequence intermittentSequence;                
    [SerializeField]private List<string> currentSymptoms = new List<string>();
    private Vector3 startMotorPosition;

    private void Start()
    {
        instanceMaterial = new Material(motorRenderer.material);
        currentColor = instanceMaterial.color;

        currentSpeedRotation = rotationSpeed;

        startMotorPosition = transform.position;
    }

    public bool GetIsMotorActive()
    {
        return isMotorActive;
    }

    public void SetSymptoms(List<string> symptoms)
    {
        if (isFixed) return;

        currentSymptoms.Clear(); 
        currentSymptoms.AddRange(symptoms);
    }

    private void ActivateSymptoms()
    {
        if (isFixed || currentSymptoms == null) return;

        Debug.Log("ActiveSympthoms");
        foreach (string symptom in currentSymptoms)
        {
            switch (symptom)
            {
                case "����":
                    StartShaftPlay();
                    break;
                case "��������":
                    StartVibrationEffect();
                    break;
                case "������":
                    StartOverheatingEffect(hotMotorColor);
                    break;
                case "��������":
                    sparkEffect.Play();
                    break;
                case "�����":
                    StartJerkingEffect();
                    break;
                case "���������� ��������":
                    isLowPowerTrouble = true;
                    break;
                case "�������":
                    StartIntermittentEffect();
                    break;
                case "����������":
                    StopedVal();
                    //isStopedTrouble= true;
                    break;
            }
        }
    }

    private void StartVibrationEffect()
    {
        if (vibrationTween == null || vibrationTween.IsPlaying() == false) 
        {
            vibrationTween = transform.DOPunchPosition(Vector3.right * powerCoefVibration, vibrationDurationInterval)
                .SetLoops(-1,LoopType.Restart)
                .Play();
        }
    }

    private void StartShaftPlay()
    {
        onlyVal.position = shaftValPosition.position; //�� fix �������� ��������� �������
    }

    private void StartOverheatingEffect(Color finalAnimColor)
    {
        motorRenderer.material = instanceMaterial;
        if (overheatingTween == null || overheatingTween.IsPlaying()==false)
        {
            overheatingTween = instanceMaterial.DOColor(finalAnimColor, overheatingDuration)
                .SetEase(Ease.Linear)
                .SetAutoKill(false)
                .Play();
        }
    }

    // ������ ������ � DOTween
    private void StartJerkingEffect()
    {
        if (jerkSequence == null || !jerkSequence.IsPlaying())
        {
            jerkSequence = DOTween.Sequence()
                            .AppendInterval(initialDelay)
                            .AppendCallback(() => StartCoroutine(JerkingCoroutine()))
                            .Play();
        }
    }

    private IEnumerator JerkingCoroutine()
    {
        while (isRunning && !isFixed)
        {
            if (valRotationTween != null && valRotationTween.IsPlaying())
            {
                valRotationTween.Pause(); // ������������� ��������
                Debug.Log("Jerking: Rotation paused at " + Time.time);
                yield return new WaitForSeconds(jerkPauseDuration); // ����� �� 2 �������
                valRotationTween.Play(); // ������������ ��������
                Debug.Log("Jerking: Rotation resumed at " + Time.time);
            }
            yield return new WaitForSeconds(jerkInterval); // �������� �� ���������� �����
        }
    }

    private void StartIntermittentEffect()
    {
        if (intermittentSequence == null || !intermittentSequence.IsPlaying())
        {
            intermittentSequence = DOTween.Sequence()
                .AppendInterval(intermittentDelay) 
                .AppendCallback(() => StartCoroutine(IntermittentCoroutine()))
                .Play();
        }
    }

    private IEnumerator IntermittentCoroutine()
    {
        while (isRunning && !isFixed)
        {
            if (valRotationTween != null && valRotationTween.IsPlaying())
            {
                float originalSpeed = currentSpeedRotation;
                valRotationTween.Pause();
                currentSpeedRotation = originalSpeed * intermittentSlowFactor; // ��������� ��������
                valRotationTween = val.DOLocalRotate(rotationAxis * currentSpeedRotation, defaultValRotationDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental)
                    .Play();
                Debug.Log("Intermittent: Speed reduced to " + currentSpeedRotation + " at " + Time.time);
                yield return new WaitForSeconds(intermittentCycle / decelerationDurationFactor); // �������� ����� �� ����������
                valRotationTween.Pause();
                currentSpeedRotation = originalSpeed; // ��������������� ��������
                valRotationTween = val.DOLocalRotate(rotationAxis * currentSpeedRotation, defaultValRotationDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental)
                    .Play();
                Debug.Log("Intermittent: Speed restored to " + currentSpeedRotation + " at " + Time.time);
            }
            yield return new WaitForSeconds(intermittentCycle); // �������� �� ���������� �����
        }
    }

    private void StopedVal()
    {
        if(isStopedNow == false)
            StartCoroutine(StartDelayStoppedVal());
    }

    private IEnumerator StartDelayStoppedVal()
    {
        yield return new WaitForSeconds(stopedDelay);

        isStopedNow = true;
        isStopedTrouble = true;

        StopMotor();
    }

    private void StartValRotation()
    {
        if(isLowPowerTrouble == true)
        {
            currentSpeedRotation = rotationSpeed / 6;
        }
        else
        {
            currentSpeedRotation = rotationSpeed; 
        }

        if(valRotationTween == null || !valRotationTween.IsPlaying())
        {
            valRotationTween = val.DOLocalRotate(rotationAxis*currentSpeedRotation,defaultValRotationDuration,RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1,LoopType.Incremental)
                .Play();
        }
    }

    // ����� ���� ���������
    private void ResetSymptoms()
    {
        sparkEffect?.Stop();
        StartOverheatingEffect(defaultMotorColor);

        isRunning = false;
        isStopedTrouble = false;
        isStopedNow = false;

        if (vibrationTween != null)
            vibrationTween.Kill();
        if (valRotationTween != null)
            valRotationTween.Kill();
        if (jerkSequence != null)
            jerkSequence.Kill();
    }

    public void ActiveMotorButtonClick()
    {
        if(!isMotorActive)
        {
            StartMotor();
        }
        else
        {
            StopMotor();
        }
    }

    private void StartMotor()
    {
        if(isStopedTrouble==false)
        {
            if (!isFixed)
            {
                Debug.Log("��������� ������� � ��������������.");
                isRunning = true;

                ActivateSymptoms();
                StartValRotation();
            }
            else
            {
                Debug.Log("��������� ������� ���������.");
                isRunning = true;
                /*ResetSymptoms();*/ // ����� ���������� �������� ���������
            }
        }

        isMotorActive = true;
    }


    private void StopMotor()
    {
        isRunning = false;
        Debug.Log("��������� ����������.");

        isMotorActive = false;

        DisableTrouble();

        if (ventilytorMain.gameObject.activeSelf)
        {
            ventilytor.rotation = val.rotation;
        }
        //ResetSymptoms(); - ����� �������������� ������ � Fix
    }

    private void DisableTrouble()
    {
        if(currentSymptoms.Contains("���� ����"))
            onlyVal.localPosition = Vector3.zero;

        sparkEffect?.Stop();
        StartOverheatingEffect(defaultMotorColor);

        isRunning = false;

        if (vibrationTween != null)
        {
            vibrationTween.Kill();
            transform.position = startMotorPosition;
        }

        if (valRotationTween != null)
            valRotationTween.Kill();
        if (jerkSequence != null)
            jerkSequence.Kill();
    }

    // ���������� �������������
    public void FixMotor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Fix"); // ������ �������� ��������/������
        }
        else
        {
            Debug.LogWarning("�������� �� ��������! ������������ ��������� ������.");
            isFixed = true;
            ResetSymptoms();
            Debug.Log("������������� ���������.");
        }
    }

    // ����� ��� ��������� ���������� �������� (���� ������������ Animator)
    public void OnFixAnimationComplete()
    {
        isFixed = true;
        //ResetSymptoms();
        Debug.Log("�������� ���������� ���������.");
    }

    public bool GetIsStopedNow()
    {
        return isStopedNow;
    }

    private void OnDestroy()
    {
        // ������� DOTween ��� ����������� �������
        if (overheatingTween != null) overheatingTween.Kill();
        if (jerkSequence != null) jerkSequence.Kill();
        if (intermittentSequence != null) intermittentSequence.Kill();
    }
}
