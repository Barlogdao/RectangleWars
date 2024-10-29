using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDataUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(SpellSO data, Action<SpellSO> onSpellSelected)
    {
        _icon.sprite = data.Image;
        _button.onClick.AddListener(() => onSpellSelected(data));
    }


}
