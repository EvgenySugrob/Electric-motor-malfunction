using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotorFaultType
{
    StatorWindingBreak, // ����� ������� �������
    WindingShortCircuit, // ��������� ����� ���������
    InterTurnShortCircuit, // ����������� ���������
    WindingToCaseShort, // ��������� ������� �� ������
    SlipRingOrBrushFailure, // ����� �� ����� ����������� ����� ��� �����
    ShaftJamming, // ������������ ����
    BearingPlay // ���� � ����������� ����
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
    public string description; // �������� ��� UI
    public List<string> symptoms; // �������� (��������, ������, �������� � �.�.)
    public List<DiagnosticToolType> diagnosticTools; // ���������� (����������, ���������� � �.�.)
    public string diagnosticResult; // ��������� ����������� (��������, "����������� �������������")
    public string fixAction; // �������� ��� ���������� (��������, "������ �������")
}
