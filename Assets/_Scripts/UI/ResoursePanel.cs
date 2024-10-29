using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResoursePanel : MonoBehaviour
{

    private HumanPlayer _player;


    [Header("Иконки ресурсов")]
    [SerializeField] Image GoldIcon;
    [SerializeField] Image ManaIcon;
    [Header("Текст количества ресурсов")]
    [SerializeField] TMPro.TextMeshProUGUI GoldText;
    [SerializeField] TMPro.TextMeshProUGUI ManaText;
    [SerializeField]
    private GameObject _goldCell, _manaCell;
    private bool _panelInitialized = false;
    private void Awake()
    {
        _goldCell.SetActive(false);
        _manaCell.SetActive(false);
    }
    public void Init(HumanPlayer player)
    {
        _player = player;
        _goldCell.SetActive(true);
        _manaCell.SetActive(true);
        GoldIcon.sprite = GameLibrary.Instance.Gold.Image;
        ManaIcon.sprite = GameLibrary.Instance.Mana.Image;
        GoldText.text = _player.Gold.ToString();
        ManaText.text = _player.Mana.ToString();
        _player.GoldChanged += OnGoldChanged;
        _player.ManaChanged += OnManaChanged;
        
        _panelInitialized = true;
    }

    private void OnManaChanged(int manaAmount)
    {
        ManaText.transform.DOScale(1.5f, 0.1f).From().OnComplete(() => ManaText.transform.DOScale(1f, 0));
        ManaText.text = manaAmount.ToString();
    }

    private void OnGoldChanged(int goldAmount)
    {
        GoldText.text = goldAmount.ToString();
        GoldText.transform.DOScale(1.5f, 0.1f).From().OnComplete(() => GoldText.transform.DOScale(1f, 0));
    }

    // Меняет отображение ресурсов при изменении количества у игрока

    private void OnDisable()
    {
        if (_panelInitialized)
        {
            _player.GoldChanged += OnGoldChanged;
            _player.ManaChanged += OnManaChanged;
        }
    }
}
