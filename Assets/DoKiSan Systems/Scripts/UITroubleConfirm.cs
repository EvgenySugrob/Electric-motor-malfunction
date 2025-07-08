using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITroubleConfirm : MonoBehaviour
{
    [SerializeField] GameObject mainTroubleWindow;
    [SerializeField] GameObject windingBreak;

    public void OpenWindow(int value)
    {
        switch (value)
        {
            case 0:
                break; 
            case 1:
                windingBreak.SetActive(true);
                break; 
            default:
                break;
        }
    }
}
