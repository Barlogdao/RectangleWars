using UnityEngine;
using DG.Tweening;

public class ForceField : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private int _damage;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void Init(Player player, int damage, float duration)
    {
        tag = player.tag;
        _damage = damage;
        _spriteRenderer.color = CompareTag("Ally") ? GameManager.Instance.CurrentPlayerColor : GameManager.Instance.Settings.EnemyPlayerColor;
        //gameObject.layer = CompareTag("Ally") ? 18 : 17;
        transform.DOScale(3f, duration).SetEase(Ease.InQuart).OnComplete(() => Destroy(gameObject));
    }

    private void Update()
    {
        transform.Rotate(0f,0f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<UnitBase>(out UnitBase unit) && unit.IsAlive && !CompareTag(collision.tag))
        {
            unit.GetTrueDamage(_damage);
        }
    }
}
