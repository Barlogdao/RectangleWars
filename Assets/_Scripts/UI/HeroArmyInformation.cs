using UnityEngine;
using UnityEngine.UI;

public class HeroArmyInformation : MonoBehaviour
{
    [SerializeField]
    Image _spellPlaceholder, _unitPlaceholder, _heroPlaceholder;
    [SerializeField]
    RectTransform _spellPlace, _unitPlace;
    private bool _isInitiated = false;

    public void Init(Hero hero)
    {
        if (_isInitiated) return;
        _heroPlaceholder.sprite = hero.HeroSprite;
        _heroPlaceholder.GetComponent<SimpleTooltip>().infoLeft = Hero.GetLeftInfo(hero);
        foreach (SpellSO spell in hero.StartSpell)
        {
            var spelldisplay = Instantiate(_spellPlaceholder, _spellPlace);
            spelldisplay.sprite = spell.Image;
            var tooltip = spelldisplay.GetComponent<SimpleTooltip>();
            tooltip.infoLeft = spell.GetLeftInfo(hero);
            tooltip.infoRight = spell.GetRightInfo(hero);
        }
        foreach (UnitDataSO unit in hero.GetHeroUnits())
        {
            var unitdisplay = Instantiate(_unitPlaceholder, _unitPlace);
            unitdisplay.sprite = unit.Image;
            var tooltip = unitdisplay.GetComponent<SimpleTooltip>();
            tooltip.infoLeft = unit.GetLeftInfo(hero);
            tooltip.infoRight = unit.GetRightInfo(hero);
        }
        _isInitiated = true;
    }


}
