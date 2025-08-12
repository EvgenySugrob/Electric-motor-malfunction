using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotationCameraInMenu : MonoBehaviour
{
    [SerializeField] Vector3 endRotate = new Vector3(0f, 45f, 0f);
    [SerializeField] float duration = 1f;

    private void Start()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        DOTween.Sequence()
            .Append(transform.DORotate(endRotate, duration, RotateMode.Fast))
            .Append(transform.DORotate(-endRotate,duration,RotateMode.Fast))
            .SetLoops(-1, LoopType.Restart)
            .Play();
    }
}
