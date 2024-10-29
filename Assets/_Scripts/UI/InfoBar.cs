using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI unitPerks;

    [SerializeField]
    UnitStatBar health, attack, armor, attackSpeed;
    [SerializeField]
    private Image unitImage, spellImage;
    [SerializeField]
    RectTransform unitInfoWindow,spellInfoWindow;
    [SerializeField]
    private TextMeshProUGUI abilityText;
    private UnitDataSO currentData;
    private Hero _hero;
    [SerializeField] private Image _classIcon;
    [SerializeField] private TextMeshProUGUI _unitName;

    private ScriptableObject _selectedata;

    private void OnEnable()
    {
        unitInfoWindow.gameObject.SetActive(false);
        spellInfoWindow.gameObject.SetActive(false);
        EventBus.NewDataObjectSelected+= OnNewDataObjectSelected;
        EventBus.HoverObjectData += OnHoverObjectData;
        EventBus.ExitObjectData+= OnExitObjectData;
       
    }
    
    public void Init(Hero hero)
    {
        _hero = hero;
    }



    private void OnNewDataObjectSelected(ScriptableObject dataObject)
    {
        _selectedata = dataObject;
        ShowSelectedDataObjectInfo();
    }
    private void OnHoverObjectData(ScriptableObject dataObject)
    {
        if (dataObject is UnitDataSO unit)
        {
            ShowUnitBar();
            DisplayUnitInfo(unit);
        }
        else if (dataObject is SpellSO spell)
        {
            ShowSpellBar();
            DisplaySpellInfo(spell);
        }
    }

    private void OnExitObjectData()
    {
        ShowSelectedDataObjectInfo();
    }


    private void ShowSelectedDataObjectInfo()
    {
        if (_selectedata is UnitDataSO unit)
        {
            ShowUnitBar();
            DisplayUnitInfo(unit);
        }
        else if (_selectedata is SpellSO spell)
        {
            ShowSpellBar();
            DisplaySpellInfo(spell);
        }
    }

    private void ShowUnitBar()
    {
        unitInfoWindow.gameObject.SetActive(true);
        spellInfoWindow.gameObject.SetActive(false);
    }
    private void ShowSpellBar()
    {
        unitInfoWindow.gameObject.SetActive(false);
        spellInfoWindow.gameObject.SetActive(true);
    }









    private void SetUnitInfo(UnitDataSO data)
    {
        unitInfoWindow.gameObject.SetActive(true);
        spellInfoWindow.gameObject.SetActive(false);
        if (currentData == null || currentData != data)
        {
            currentData = data;
            DisplayUnitInfo(data);
        }
    }

    private void DisplayUnitInfo(UnitDataSO unit)
    {
        unitImage.sprite = unit.Image;
        health.SetStat(unit.Health.ToString());
        attack.SetStat(unit.Attack.ToString());
        armor.SetStat(unit.Armor.ToString());
        attackSpeed.SetStat(unit.AttackSpeed.ToString());
        unitPerks.text = unit.PerkInfo.ToString();
        _classIcon.sprite = GameLibrary.Instance.Fractions.GetClassConfig(unit.Class).ClassIcon;
        _unitName.text = unit.Name;
    }

    private void ShowUnitInfo(UnitDataSO data)
    {
        unitInfoWindow.gameObject.SetActive(true);
        if (currentData == null || data != currentData)
        DisplayUnitInfo(data);
    }
    private void ExitUnitInfo(UnitDataSO data)
    {
        
        if (currentData != null && data != currentData)
        {
            DisplayUnitInfo(currentData);
        }
        else if (currentData == null)
        {
            unitInfoWindow.gameObject.SetActive(false);
        }
    }

    private void DisplaySpellInfo(SpellSO spell)
    {
        spellImage.sprite = spell.Image;
        abilityText.text = spell.Name + "\n\n\n" + spell.Description(_hero);
    }
    private void OnPointerEnterAbility(SpellSO spell)
    {
        unitInfoWindow.gameObject.SetActive(false);
        spellInfoWindow.gameObject.SetActive(true);
        spellImage.sprite = spell.Image;
        abilityText.text = spell.Name +"\n\n\n"+ spell.Description(_hero);
    }
    private void OnPointerLeaveAbility()
    {
        spellInfoWindow.gameObject.SetActive(false);
        unitInfoWindow.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        EventBus.NewDataObjectSelected -= OnNewDataObjectSelected;
        EventBus.HoverObjectData -= OnHoverObjectData;
        EventBus.ExitObjectData -= OnExitObjectData;
    }
}
