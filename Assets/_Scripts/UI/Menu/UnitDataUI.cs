using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitDataUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(UnitDataSO data, Action<UnitDataSO> onUnitSelected)
    {
        _icon.sprite = data.Image;
        _button.onClick.AddListener(() =>onUnitSelected(data));
    }

    
}
