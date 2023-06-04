using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private float scaleMultiplier = 1.2f;

    private Vector3 originalScale;
    private Tweener tweener;

    public void ScaleEffect(GameObject target)
    {
        if (tweener != null && tweener.IsActive())
        {
            //tweener.Kill();
            target.transform.localScale = originalScale;
        }
        originalScale = transform.localScale;

        // 버튼 클릭 시 이펙트 실행
        tweener = target.transform.DOScale(originalScale * scaleMultiplier, scaleDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => target.transform.DOScale(originalScale, scaleDuration)
                .SetEase(Ease.OutBack));
    }
    
}
