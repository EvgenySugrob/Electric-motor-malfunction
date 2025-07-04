using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotorFaultType
{
    StatorWindingBreak, // Обрыв обмотки статора
    WindingShortCircuit, // Замыкание между обмотками
    InterTurnShortCircuit, // Межвитковое замыкание
    WindingToCaseShort, // Замыкание обмотки на корпус
    SlipRingOrBrushFailure, // Выход из строя токосъемных колец или щеток
    ShaftJamming, // Заклинивание вала
    BearingPlay // Люфт в подшипниках вала
}

public enum DiagnosticToolType
{
    Multimeter,
    Megohmmeter,
    ManualCheck
}

[System.Serializable]
public class MotorFaultScenario
{
    public MotorFaultType faultType;
    public string description; // Описание для UI
    public List<string> symptoms; // Симптомы (вибрация, нагрев, искрение и т.д.)
    public List<DiagnosticToolType> diagnosticTools; // Инструмент (мультиметр, мегаомметр и т.д.)
    public string diagnosticResult; // Результат диагностики (например, "бесконечное сопротивление")
    public string fixAction; // Действие для устранения (например, "замена обмотки")
}
