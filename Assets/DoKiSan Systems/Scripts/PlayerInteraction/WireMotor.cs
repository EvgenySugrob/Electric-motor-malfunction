using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WireMotor : MonoBehaviour
{
    [Header("ControlPoint")]
    [SerializeField] Transform endPosition;
    [SerializeField] float duration = 0.5f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void RemoveWireMotor(bool isInvert)
    {
        if(isInvert)
        {
            gameObject.SetActive(true);

            transform.DOMove(startPosition,duration)
                .Play();
        }
        else
        {
            StartCoroutine(WaitAnimationForDisable());
        }
    }

    private IEnumerator WaitAnimationForDisable()
    {
        yield return transform.DOMove(endPosition.position, duration)
                .Play()
                .WaitForCompletion();

        gameObject.SetActive(false);
    }
}
