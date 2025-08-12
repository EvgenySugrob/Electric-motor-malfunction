using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vmaya.UI.Components;

public class RandomCodeControlWork : MonoBehaviour
{
    [SerializeField] SelectingLabWork selectingLabWork;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] int secretKey = 37;
    [SerializeField] PlaceholderAnim placeholderAnim;

    public void ControlLabClick()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        if(!int.TryParse(inputField.text, out int inputNumber))
        {
            Debug.LogWarning("¬ведено не число!");
            return;
        }

        int labNumber = (inputNumber * secretKey) % 8;

        Debug.Log($"{inputNumber*secretKey} - скобки. {(inputNumber * secretKey)%8} - остаток");

        selectingLabWork.OnExamButtonClicked(labNumber);
    }

    public void SetRandomNumber()
    {
        int number = Random.Range(0, 1000);
        placeholderAnim.StartAnimRandomNumber();
        inputField.text = number.ToString();
    }
}
