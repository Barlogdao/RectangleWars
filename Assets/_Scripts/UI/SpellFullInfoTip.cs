using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellFullInfoTip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _spellImage, _manaImage;
    [SerializeField] TMPro.TextMeshProUGUI _spellNameLabel, _spellDescription, _manaAmount;
    public static Action<SpellSO> ShowInfo;
    private void Awake()
    {
        ShowInfo += SetSpell;
        gameObject.SetActive(false);
    }

    private void SetSpell(SpellSO spell)
    {
        gameObject.SetActive(true);
        _spellImage.sprite = spell.Image;
        _spellNameLabel.text = spell.Name;
        _manaImage.sprite = GameLibrary.Instance.Mana.Image;
        _manaAmount.text = spell.ManaCost.ToString();
        _spellDescription.text = spell.Description(GameLibrary.Instance.HeroList[0].hero);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ShowInfo -= SetSpell;
    }
}
