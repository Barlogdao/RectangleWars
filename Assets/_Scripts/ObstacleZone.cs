using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Extensions;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ObstacleZone : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = CompareTag("Ally") ? GameManager.Instance.CurrentPlayerColor : GameManager.Instance.Settings.EnemyPlayerColor;
        _spriteRenderer.color = _spriteRenderer.color.WithA(0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _spriteRenderer.color = _spriteRenderer.color.WithA(0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = _spriteRenderer.color.WithA(0f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_spriteRenderer.color.a == 0f)
        {
            _spriteRenderer.DOFade(0.4f, 0.1f).OnComplete(() => _spriteRenderer.DOFade(0f, 0.1f)).SetUpdate(true);
        }
    }

}
