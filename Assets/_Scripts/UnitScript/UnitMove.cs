using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{

    private UnitBase unit;
    private Rigidbody2D rb;

    private SpriteRenderer _sr;
    private UnitAnimationModule _animationModule;
    private IEnumerator freeze;
    private bool _isImmobilized = false;
    private bool _isStunned = false;
    public bool IsImmobilized { get => _isImmobilized; }

    //private bool facingRight = true;
    private float UnitSpeed { get => unit.Speed; }
    private Vector2 savedDirection;
    public bool CanMove { get => !unit.InBattle && !IsImmobilized && !unit.IsBusy && !_isStunned; }
    private void Awake()
    {
        unit = GetComponent<UnitBase>();
        rb = GetComponent<Rigidbody2D>();

        _sr = GetComponent<SpriteRenderer>();
        _animationModule = GetComponent<UnitAnimationModule>();
        unit.LocalDieEvent += OnUnitDie;
    }

    private void OnUnitDie(UnitBase owner)
    {
        owner.LocalDieEvent -= OnUnitDie;
        StopAllCoroutines();
    }

    public void Go()
    {
        if (CanMove)
        {
            if (savedDirection == Vector2.zero)
            {
                RandomMove();
            }
            else
            {
                rb.velocity = savedDirection * UnitSpeed;
                savedDirection = Vector2.zero;
                _animationModule.PerformWalk(unit.Speed / unit.Data.Speed);
                CheckDirection();
            }
            
           
            
        }
    }
    public void Stop()
    {
        if (CanMove)
        {
            savedDirection = rb.velocity.normalized;
            rb.velocity = Vector2.zero;
            _animationModule.PerformIdle();
        }
    }

    public void OnChangeSpeed()
    {
        if (CanMove)
        {
            rb.velocity = rb.velocity.normalized * UnitSpeed;
            _animationModule.ChangeAnimationSpeed(unit.Speed / unit.Data.Speed);
        }
    }
    public void MoveTo(Vector3 target)
    {
        if (CanMove)
        {
            rb.velocity = (target - unit.transform.position).normalized * UnitSpeed;
            CheckDirection(target);
            _animationModule.PerformWalk(unit.Speed / unit.Data.Speed);
        }
    }
    public void RandomMove()
    {
        if (CanMove)
        {
            rb.velocity = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)).normalized * UnitSpeed;
            CheckDirection();
            _animationModule.PerformWalk(unit.Speed / unit.Data.Speed);
        }
    }
    public void CheckDirection()
    {
        //if (rb.velocity.x < 0 && facingRight) Flip();
        //else if (rb.velocity.x > 0 && !facingRight) Flip();
        _sr.flipX = rb.velocity.x < 0;
    }
    public void CheckDirection(Vector2 target) {
        //if (transform.position.x > target.x && facingRight) Flip();
        //else if (transform.position.x < target.x && !facingRight) Flip();
        _sr.flipX = transform.position.x > target.x;
    }

    public void ImmobilizeUnit(float seconds)
    {
        if (freeze != null)
            StopCoroutine(freeze);
        freeze = FreezeUnit(seconds);
        StartCoroutine(freeze);
    }
    private  IEnumerator FreezeUnit(float seconds)
    {
        Stop();
        _isImmobilized = true;
        yield return new WaitForSeconds(seconds);
        _isImmobilized = false;

        Go();
    }

    public void Stun()
    {
        Stop();
        _isStunned = true;
    }
    public void UnStun()
    {
        _isStunned = false;
        Go();
    }



    //private void Flip()
    //{
    //    Vector3 currentScale = transform.localScale;
    //    currentScale.x *= -1;
    //    transform.localScale = currentScale;
    //    facingRight = !facingRight;

    //}

}
