using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField]
    AbilityInfo abilityInfoPrefab;
    private List<AbilityInfo> _spellList;
    private AbilityInfo _selectedSpell;
    private HumanPlayer _player;
    public void InitBar(Hero hero, HumanPlayer player)
    {
        _player = player;
        _spellList = new List<AbilityInfo>();
        CreateSpellBars(hero, player);
        if (_spellList.Count > 0)
        {
            _spellList.ForEach(spell => spell.SpellSelected += OnSpellInfoSelected);
            PlayerInputController.Instance.CastSpellPressed += OnCastSpellPressed;
            PlayerInputController.Instance.SpellSelectionPressed += OnSpellSelectionPressed;
            _selectedSpell = _spellList[0];
            _selectedSpell.SelectSpell();
        }
    }

    private void OnSpellInfoSelected(AbilityInfo selected)
    {
        if (selected == null) return;
        if (_selectedSpell != selected) _selectedSpell = selected;
        foreach (var spellInfo in _spellList)
        {
            if (spellInfo != _selectedSpell)
            {
                spellInfo.DeselectSpell();
            }
        }
    }

    private void OnSpellSelectionPressed(int value)
    {
        _selectedSpell.DeselectSpell();
        if (value > 0)
        {
            _selectedSpell = _spellList[(GetIndexOfSpell(_selectedSpell) + 1) % _spellList.Count];
        }
        else
        {
            _selectedSpell = _spellList[GetIndexOfSpell(_selectedSpell) - 1 >= 0 ?
                GetIndexOfSpell(_selectedSpell) - 1 :
                _spellList.Count - 1];
        }

        _selectedSpell.SelectSpell();
    }
    private int GetIndexOfSpell(AbilityInfo spell)
    {
        return _spellList.IndexOf(spell);
    }

    private void OnCastSpellPressed()
    {
        _selectedSpell.UseSpell();
    }

    private void CreateSpellBars(Hero hero, HumanPlayer player)
    {
        for (int i = 0; i < hero.StartSpell.Count; i++)
        {
            var prefab = Instantiate(abilityInfoPrefab, transform);
            prefab.InitAbilityBar(KeyCode.Alpha1 + i, player, hero.StartSpell[i]);
            _spellList.Add(prefab);
        }
    }
}
