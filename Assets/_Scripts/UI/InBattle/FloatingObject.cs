using UnityEngine;
using DG.Tweening;

public class FloatingObject : MonoBehaviour
{

    private Tweener _tween;
    private Tweener _tween2;
    [SerializeField] private float _tweenDuration;
    private void OnEnable()
    {
        var tr = transform as RectTransform;
        _tween = transform.DOScale(1.1f, _tweenDuration).SetLoops(-1, LoopType.Yoyo);
        _tween2 = tr.DOAnchorPosY(10f, _tweenDuration).SetRelative(true).SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDisable()
    {
        _tween2.Kill();
        _tween.Kill();
    }

}
