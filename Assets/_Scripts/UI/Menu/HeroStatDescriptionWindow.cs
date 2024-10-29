using UnityEngine.UI;
using UnityEngine;
using RB.HeroStats;
using Coffee.UIEffects;

public class HeroStatDescriptionWindow : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _statName;
    [SerializeField] Image _insightImage;
    [SerializeField] TMPro.TextMeshProUGUI[] _statEffectsText;
    [SerializeField] TMPro.TextMeshProUGUI _unitPerkText;
    [SerializeField] Image[] _pointImages;
    [SerializeField] Color _unlockedColor, _lockedColor;
    private SimpleTooltip _insightTooltip;
    private SimpleTooltip _unitperkTooltip;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _insightTooltip = _insightImage.gameObject.AddComponent<SimpleTooltip>();
        _unitperkTooltip = _unitPerkText.gameObject.AddComponent<SimpleTooltip>();
    }

    public void ShowHeroStat(HeroStatSO stat,Hero hero)
    {
        gameObject.SetActive(true);
        // определяем уровень навыка
        int level = 0;
        switch (stat.Stat)
        {
            case HeroStat.Leadership:
                level = hero.Leadership;
                break;
            case HeroStat.Sorcery:
                level = hero.Sorcery;
                break;
            case HeroStat.Stamina:
                level = hero.Stamina;
                break;
        }
        
        // Устанавливаем заголовок - название навыка
        _statName.text = stat.Name;
        _statName.color = stat.StatColor;
        //Обнуляем цвет описания эффекта навыка  и камешков
        foreach(var statText in _statEffectsText)
        {
            statText.color = _lockedColor;
        }
        foreach(var poimtImage in _pointImages)
        {
            poimtImage.color = new Color(1f,1f,1f,0f);
        }
        // Устанавливаем цвет описания эффекта навыка в зависимости от уровня
        for (int i = 0; i < stat.StatEffects.Length; i++)
        {
            _statEffectsText[i].text = stat.StatEffects[i].ShowDescription();
        }
        // Устанавливаем цвет камешков в зависимости от навыка
        for (int i = 0; i< level; i++)
        {
            _statEffectsText[i].color = _unlockedColor;
            _pointImages[i].color = stat.StatColor;
            
            _pointImages[i].GetComponent<UIShiny>().Play();
            
        }

        //устанавливаем Инсайт и его подсказку
        var insight = stat.StatEffects[^1] as InsightEffect;
        _insightImage.sprite = insight.Insight.Image;
        _insightTooltip.infoLeft = insight.Insight.GetLeftInfo(hero);
        // Устанавливаем подсказку перка
        var unitPerk = stat.StatEffects[2] as UnitPerkBuffEffect;
        _unitperkTooltip.infoLeft = unitPerk.Perk.Description;
        _unitPerkText.text = unitPerk.Perk.Name;
        _unitPerkText.color = level >= 3 ? _unlockedColor : _lockedColor;

        _exitButton.Select();

    }

}
