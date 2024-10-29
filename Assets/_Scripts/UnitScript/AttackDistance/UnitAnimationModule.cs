using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class UnitAnimationModule : MonoBehaviour
{
    private GameObject _bulletPrefab;
    private AttackDistanceType _type;
    private UnitBase _unit;
    private Animator _animator;


    public void Init (AttackDistanceType type,UnitBase unit, GameObject bulletPrefab, RuntimeAnimatorController animator)
    {
        _type = type;
        _unit = unit;
        _bulletPrefab = bulletPrefab;
        _animator= GetComponent<Animator>();
        _animator.runtimeAnimatorController = animator;
    }
    public void PerformAttack()
    {
        _animator.StopPlayback();
        _animator.speed = 1f;
        _animator.Play(AnimatorStates.Attack);
    }
    public void PerformIdle()
    {
        _animator.StopPlayback();
        _animator.speed = 1f;
        _animator.Play(AnimatorStates.Idle);
    }
    public void PerformWalk(float speed)
    {
        _animator.speed = speed;
        _animator.Play(AnimatorStates.Walk);
    }

    public void PerformDeath()
    {
        _animator.StopPlayback();
        _animator.speed = 1f;
        _animator.Play(AnimatorStates.Die);
    }
    public void PerformSpecial()
    {
        _animator.StopPlayback();
        _animator.speed = 1f;
        _animator.Play(AnimatorStates.Special);
    }
    public void ChangeAnimationSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void PerformBullet(int bulletLifetime)
    {
        if (_bulletPrefab != null && _unit.EnemyTransform != null)
        {
            switch (_type)
            {
                case AttackDistanceType.None:
                    break;
                case AttackDistanceType.Melee:
                    break;
                case AttackDistanceType.Range:
                    var bullet = Instantiate(_bulletPrefab,_unit.transform.position.AddY(_unit.SpriteHeight/2), Quaternion.identity);
                    bullet.transform.LookAtTarget(_unit.EnemyTransform.position, SpriteFrontSide.Right);
                    bullet.transform.DOMove(_unit.EnemyTransform.position.AddY(0.5f), bulletLifetime == 0 ?0.1f:FramesToSeconds(bulletLifetime)).OnComplete(()=> Destroy(bullet));
                    break;
                case AttackDistanceType.Artillery:
                    var siegebullet = Instantiate(_bulletPrefab, _unit.EnemyTransform.position.AddY(_unit.SpriteHeight / 2), Quaternion.identity);
                    siegebullet.transform.DOScale(3f, 0.3f);
                    break;
            }
        }
    }
    public void PerformDamage()
    {
        if (_unit.EnemyTarget != null && _unit.EnemyTarget.IsAlive)
        _unit.DealDamage(_unit.EnemyTarget);
    }

    public float GetAnimationLenght()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private float FramesToSeconds(int frameAmount) => frameAmount / _animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;


}
