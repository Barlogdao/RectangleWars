using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;

public class HeroHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthbar,_manaBar, _heroIcon, _insightBar;
    [SerializeField] private UIShadow _frameOutline;
    [SerializeField] private UIShiny _frameShine;
    private Player _player;
    private Color _manaColor,_insightColor;
    
    
    

    public void Init(Color color, Player player)
    {
        _player = player;
        _manaColor = _manaBar.color;
        _insightColor = _insightBar.color;
        _healthbar.color = color;
        _healthbar.fillAmount = 1f;
        _insightBar.fillAmount = 0f;
        _manaBar.fillAmount = (float)_player.Mana/_player.MaxMana;
        _heroIcon.sprite = player.Hero.HeroSprite;
        if (player is not HumanPlayer) _heroIcon.transform.localScale = new Vector3 (-1,1,1);
        
        
        _player.BattlefieldHero.HeroHealthChanged += OnHealthChanged;
        _player.ManaChanged += OnManaChanged;
        _player.InsightChanged += OnInsightChanged;
        transform.DOScale(0f, 0f).OnComplete(() => transform.DOScale(1f, 1f).SetUpdate(true).SetEase(Ease.OutQuad));
    }

    private void OnInsightChanged(int currentInsight)
    {
        _insightBar.DOFillAmount((float)currentInsight / 100, 0.13f);
        _insightBar.DOColor(Color.white, 0.13f).From().OnComplete(() => _insightBar.color = _insightColor);
    }

    private void OnManaChanged(int currentMana)
    {
        _manaBar.DOFillAmount((float)currentMana / _player.MaxMana, 0.13f);
        _manaBar.DOColor(Color.white, 0.13f).From().OnComplete(() => _manaBar.color = _manaColor);
        //_manaBar.fillAmount = (float)currentMana / _player.MaxMana;
    }

    public void OnHealthChanged(int currentHealth)
    {
        if (_player.BattlefieldHero.GetLethalDamage)
        {
            StopFrameShining();
        }
        //_healthbar.fillAmount = (float)currentHealth/ _player.BattlefieldHero.MaxHealth;
        _healthbar.DOFillAmount((float)currentHealth / _player.BattlefieldHero.MaxHealth, 0.13f);
        _healthbar.DOColor(Color.white, 0.13f).From().OnComplete(()=> _healthbar.color = _player.PlayerColor);
    }

    private void StopFrameShining()
    {
        _frameOutline.enabled = false;
        _frameShine.enabled = false;
    }

    private void OnDestroy()
    {
        _player.BattlefieldHero.HeroHealthChanged -= OnHealthChanged;
        _player.ManaChanged -= OnManaChanged;
        _player.InsightChanged -= OnInsightChanged;
    }


}
