using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMoveWhenExamWindowOpen : MonoBehaviour
{
    [Header("CameraController")]
    [SerializeField] CameraController controller;
    [SerializeField] MouseCursorHandler mouseCursorHandler;
    
    public void DisableCameraController(bool isState)
    {
        controller.enabled = isState;
        mouseCursorHandler.enabled = isState;
    }
}
