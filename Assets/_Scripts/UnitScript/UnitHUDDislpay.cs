using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UnitHUDDislpay : MonoBehaviour
{
    [SerializeField]
    Image _attackSpeedDisplay, _healthDisplay;
    [SerializeField]
    CanvasGroup _healthgroup;
    Tween tween;

    private void Start()
    {
        _attackSpeedDisplay.fillAmount = 0f;
        _healthgroup.alpha = 0;
        UnitBase unit = GetComponentInParent<UnitBase>();
        unit.StartAttack += OnStartAttack;
        unit.LocalDieEvent += OnUnitDie;
        unit.UnitDamaged += OnUnitDAmaged;
    }

    private void OnUnitDAmaged(int currentHealth, int maxHealth)
    {
        _healthgroup.alpha = 1f;
       _healthDisplay.fillAmount = (float)currentHealth/maxHealth;
        if (currentHealth == maxHealth)
        {
            _healthgroup.DOFade(0f, 1f);
        }
    }

    private void OnStartAttack(float attackDuration)
    {
        tween = _attackSpeedDisplay.DOFillAmount(1f, attackDuration).OnComplete(()=> _attackSpeedDisplay.fillAmount = 0f);
    }

    private void OnUnitDie(UnitBase unit)
    {
        tween.Kill();
        _healthgroup.alpha = 0f;
        _attackSpeedDisplay.fillAmount = 0f;
        unit.StartAttack -= OnStartAttack;
        unit.UnitDamaged -= OnUnitDAmaged;
        unit.LocalDieEvent -= OnUnitDie;
    }
}
