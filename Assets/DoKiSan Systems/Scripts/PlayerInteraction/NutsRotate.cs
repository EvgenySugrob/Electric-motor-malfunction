using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NutsRotate : MonoBehaviour
{
    [SerializeField] Vector3 nutsLocalRotation = new Vector3(0, 0, -720f);
    [SerializeField] float duration = 0.5f;

    public void RotationNuts(bool isInvetr)
    {
        if(isInvetr)
        {
            StartCoroutine(WaitAnimationBack());
        }
        else
        {
            transform.DOLocalRotate(nutsLocalRotation, duration, RotateMode.LocalAxisAdd)
                .Play();
        }
    }

    private IEnumerator WaitAnimationBack()
    {
        yield return new WaitForSeconds(2f);

        transform.DOLocalRotate(-nutsLocalRotation, duration, RotateMode.LocalAxisAdd)
                .Play();
    }
}
