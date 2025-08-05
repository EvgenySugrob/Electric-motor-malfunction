using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoggingUserActions : MonoBehaviour
{
    public static LoggingUserActions Instance { get; private set; }

    [Header("Logging")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject prefabTextSpawn;
    private TMP_Text prefabText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddUserActionInLog(string actionText)
    {
        if (string.IsNullOrEmpty(actionText))
            return;

        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        GameObject spawnLogText = Instantiate(prefabTextSpawn, spawnPoint);
        spawnLogText.transform.SetAsFirstSibling();

        prefabText = spawnLogText.GetComponent<TMP_Text>();
        prefabText.text = $"{currentTime} - {actionText}";

        spawnLogText.SetActive(true);
    }
}
