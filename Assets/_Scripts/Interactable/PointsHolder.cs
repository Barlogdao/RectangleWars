using UnityEngine;
using DG.Tweening;


public class PointsHolder : MonoBehaviour
{
    [SerializeField] private  SpriteRenderer[] _pointImages;
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;  
        HidePointsVisual();
    }
    public void ShowPointsVisual(int count)
    {
        transform.DOScale(_originalScale * 2, 0.2f).OnComplete(() => transform.DOScale(_originalScale, 0.2f));
        for (int i = 1; i <= _pointImages.Length; i++)
        {
            if(count >= i)
            {
                _pointImages[i-1].enabled = true;
            }
            else
            {
                _pointImages[i-1].enabled = false;
            }
        }
    }
    
    public void HidePointsVisual()
    {
        foreach (var point in _pointImages)
        {
            point.enabled = false;
        }
    }

}
