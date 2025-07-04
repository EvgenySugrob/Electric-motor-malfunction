using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faults/����� ������� �������")]
public class OpenWindingFault : FaultScenario
{
    private HashSet<(string, string)> brokenConnections;

    public override void InitializeScenario()
    {
        brokenConnections = new HashSet<(string, string)>();

        // �������� ������� 1 ��� 2 "������"
        var candidates = new List<(string, string)>
        {
            ("U2", "V2"),
            ("V2", "W2"),
            ("U2", "W2")
        };

        int count = Random.Range(1, 3); // ������� ������ ������� (1 ��� 2)

        for (int i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, candidates.Count);
            brokenConnections.Add(candidates[randomIndex]);
            candidates.RemoveAt(randomIndex);
        }
    }

    public override string GetMeasurementResult(string pointA, string pointB)
    {
        var pair = (pointA, pointB);
        var reversed = (pointB, pointA);

        if (brokenConnections.Contains(pair) || brokenConnections.Contains(reversed))
            return "�����";

        return "5 ��"; // ���������� �������������
    }
}
