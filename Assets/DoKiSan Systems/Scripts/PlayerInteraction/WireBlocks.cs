using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WireBlocks : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] float duration = 0.5f;
    [SerializeField] private float startWeightValue = 0f;
    [SerializeField] private float endWeightValue = 100f;

    public void RemoveWires(bool isInvert)
    {
        if(isInvert)
        {
            gameObject.SetActive(true);

            DOTween.To(() => skinnedMeshRenderer.GetBlendShapeWeight(0),
                x => skinnedMeshRenderer.SetBlendShapeWeight(0, x),
                startWeightValue,
                duration)
                .Play();
        }
        else
        {
            StartCoroutine(WaitAnimationForDisable());
        }
    }

    private IEnumerator WaitAnimationForDisable()
    {
        yield return DOTween.To(() => skinnedMeshRenderer.GetBlendShapeWeight(0),
                x => skinnedMeshRenderer.SetBlendShapeWeight(0, x),
                endWeightValue,
                duration)
                .Play()
                .WaitForCompletion();

        gameObject.SetActive(false);
    }
}
