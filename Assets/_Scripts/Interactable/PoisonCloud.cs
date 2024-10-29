using System.Collections;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    [SerializeField] public BoxCollider2D BoxCollider2D;
    private int _damage;
    private Player _owner;
    private float _duration;
    public void Init(Player owner, int damage, float duration)
    {
        _owner = owner;
        _damage = damage;
        _duration = duration;
    }
    private IEnumerator Start()
    {
        yield return Utilis.GetWait(_duration);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitBase>(out UnitBase target)
            && target.Owner != _owner
            && !target.ImmuneToMagic)
        {
            target.StartCoroutine(PoisonEffect(target, collision));
        }
    }

    private IEnumerator PoisonEffect(UnitBase target, Collider2D collision)
    {
        while (collision!= null && BoxCollider2D != null && collision.IsTouching(BoxCollider2D))
        {
            target.GetTrueDamage(_damage);
            yield return Utilis.GetWait(1f);
        }
    }
}
