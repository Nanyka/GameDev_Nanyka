using System.Collections;
using PrimeTween;
using UnityEngine;

namespace TheAiAlchemist
{
    public class UIShrink : MonoBehaviour
    {
        [SerializeField] private float shrinkDelay = 0.5f;

        private IEnumerator Start()
        {
            var rectComponent = GetComponent<RectTransform>();
            yield return new WaitForSeconds(shrinkDelay);

            Sequence.Create()
                .Chain(Tween.LocalScale(rectComponent, new Vector3(1.15f, 0.9f, 1.15f), 0.1f, Ease.OutSine,
                    1, CycleMode.Yoyo))
                .Chain(Tween.LocalScale(rectComponent, new Vector3(0.9f, 1.15f, 0.9f), 0.1f, Ease.OutSine,
                    1, CycleMode.Yoyo))
                .Chain(Tween.LocalScale(rectComponent, new Vector3(1.15f, 0.9f, 1.15f), 0.1f, Ease.OutSine,
                    1, CycleMode.Yoyo))
                .Chain(Tween.LocalScale(rectComponent, new Vector3(1f, 1f, 1f), 0.1f, Ease.OutSine, 
                    1, CycleMode.Yoyo))
                .ChainDelay(1);
        }
    }
}