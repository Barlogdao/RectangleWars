using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.SimpleLocalization;

public class NewGameWindow : MonoBehaviour
{
    [SerializeField] TMP_Dropdown GameCompexity;
    [SerializeField] Image colorImage, heroImage;
    List<HeroSO> herolist;
    HeroSO currentHero;
    private GamePlayersCondition _condition;
    [SerializeField] TextMeshProUGUI HeroName, HeroFraction, Leadership, Sorcery, Stamina;
    [SerializeField] private HeroStatPoints _leadrshipPoints, _sorseryPoints, _staminaPoints;
    [SerializeField] InsightToolTip _insightToolTip;
    [SerializeField] HeroStatDescriptionWindow _statDescriptionWindow;
    [SerializeField] Toggle _cheatInventoryToggle;

    void Start()
    {
        herolist = GameLibrary.Instance.HeroList;
        currentHero = herolist[Random.Range(0, herolist.Count -1)];
        _insightToolTip.Init(currentHero.hero, currentHero.hero.Insights[0], currentHero.hero.Insights[0].Image);
        RefreshHero();
        HeroStatTooltip.HeroStatClicked += OnHeroStatClicked;
    }

    private void OnHeroStatClicked(HeroStatSO stat)
    {
        _statDescriptionWindow.ShowHeroStat(stat, currentHero.hero);
    }

    private void OnEnable()
    {
        if(currentHero != null) RefreshHero();
    }

    public void NewGame() 
    {
        GameManager.Instance.CurrentPlayerColor = colorImage.color;
        GameManager.Instance.GameComplexity = (Complexity)GameCompexity.value;
        GameManager.Instance.SetNewHero(currentHero.hero, _cheatInventoryToggle.isOn);
        GameManager.Instance.Condition = _condition;
        EventBus.NewGameEvent.Invoke(); 
    }
    public void NextHero()
    {
        currentHero = herolist[(herolist.IndexOf(currentHero) + 1) % herolist.Count];
        RefreshHero();
    }
    public void PreviousHero()
    {
       currentHero = herolist.IndexOf(currentHero) - 1 < 0 ?
                     herolist[^1] :
                     herolist[herolist.IndexOf(currentHero) - 1];
        RefreshHero();
    } 
    public void SetGameConditions(bool AIVSAI)
    {
        if (AIVSAI) _condition = GamePlayersCondition.AiVsAi;
        else _condition = GamePlayersCondition.humanVsAi;
    }
    void RefreshHero()
    {
        heroImage.sprite = currentHero.hero.HeroSprite;
        HeroName.text = LocalizationManager.Localize(currentHero.hero.HeroNameKey); 
        HeroFraction.text = LocalizationManager.Localize($"Fraction.{currentHero.hero.fraction}");
        Leadership.text = $"{LocalizationManager.Localize("Hero.Leadership")}: {currentHero.hero.Leadership}";
        Sorcery.text = $"{LocalizationManager.Localize("Hero.Sorcery")}: {currentHero.hero.Sorcery}";
        Stamina.text = $"{LocalizationManager.Localize("Hero.Stamina")}: {currentHero.hero.Stamina}";
        _leadrshipPoints.SetPoints(currentHero.hero.Leadership);
        _sorseryPoints.SetPoints(currentHero.hero.Sorcery);
        _staminaPoints.SetPoints(currentHero.hero.Stamina);
        _insightToolTip.Init(currentHero.hero, currentHero.hero.Insights[0], currentHero.hero.Insights[0].Image);

    }
    private void OnDestroy()
    {
        HeroStatTooltip.HeroStatClicked -= OnHeroStatClicked;
    }
}
