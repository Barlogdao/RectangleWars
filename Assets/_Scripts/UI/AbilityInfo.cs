using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Coffee.UIEffects;


public class AbilityInfo : MonoBehaviour
{
    private SpellSO _spellData;
    public SpellSO SpellData => _spellData;
    [SerializeField]
    private Image _panel; // отображение панели
    [SerializeField]
    CanvasGroup _panelCanvasGroup;
    Button button;
    private HumanPlayer _player;
    [SerializeField]
    RectTransform _abilityInfoTransform;

    [Header("Текст ресурсов")]
    [SerializeField] TextMeshProUGUI manaCost;
    [Header("Изображение умения")]
    [SerializeField] Image abilityImage;
    [Header("Цвета рамки")]
    [SerializeField] Color enableColor;
    [SerializeField] Color disableColor;
    [SerializeField] Color _freeSpellImageColor;
    KeyCode UseKey;
    [SerializeField]
    TextMeshProUGUI keyDisplay;
    [SerializeField] private UIShiny _frameShine;

    private Tweener _freeSpellTweener = null;



    public bool HaveManaForCast => _player.HaveManaforCast(_spellData);
    public event Action<AbilityInfo> SpellSelected;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(UseKey))
        {
            SelectSpell();
            UseSpell();
        }
    }

    public void InitAbilityBar(KeyCode key, HumanPlayer player, SpellSO spelldata)
    {
        _player= player;
        _spellData = spelldata;
        abilityImage.sprite = _spellData.Image;
        manaCost.text = (_spellData.ManaCost - _player.SpellCostReduce).ToString();
        UseKey = key;
        keyDisplay.text = UseKey.ToString()[^1].ToString();

        button.onClick.AddListener(SelectSpell);
        _player.ManaChanged += OnManaAmountChanged;
        PlayerInputController.ChangedOnGamepad += OnChangedToGamepad;

        _player.FreeSpellCounterChanged += OnFreeSpellCounterChanged;
        OnFreeSpellCounterChanged(_player.FreeSpellCounter);

    }

    private void OnFreeSpellCounterChanged(int counterValue)
    {
        if(_spellData.Tier <= counterValue)
        {
            manaCost.enabled = false;
            _freeSpellTweener ??= abilityImage.DOColor(_freeSpellImageColor,1f).SetLoops(-1,LoopType.Yoyo).OnKill(()=>abilityImage.color = Color.white);
        }
        else
        {
            manaCost.enabled = true;
            _freeSpellTweener?.Kill();
            _freeSpellTweener = null;
                
        }
    }

    private void OnChangedToGamepad(bool value)
    {
        keyDisplay.text = value == true ? "" : UseKey.ToString()[^1].ToString();
    }

    private void OnManaAmountChanged(int currentMana)
    {
        _panel.color = HaveManaForCast ? enableColor : disableColor;
        _frameShine.enabled = _panel.color == enableColor;
    }

    public void UseSpell()
    {

        if (_player.CanCastSpell(_spellData))
        {
            _player.CastSpell(_spellData);
  
            Recharge(_player.LastCastedTimeOfSpells[_spellData] + _spellData.SpellCooldown - Time.time);
        }
    }

  

    public void PointerEnterAbility()
    {
        EventBus.HoverObjectData?.Invoke(_spellData);
    }
    public void PointerLeaveAbility()
    {
        EventBus.ExitObjectData?.Invoke();
    }
    public void Recharge(float cd)
    {
        
        abilityImage.fillAmount = 0f;
        abilityImage.DOFillAmount(1, cd - 0.2f);  
    }

    public void SelectSpell()
    {
        _panelCanvasGroup.alpha = 1f;
        _abilityInfoTransform.DOScale(1.15f, 0.15f);
        EventBus.NewDataObjectSelected(_spellData);
        SpellSelected?.Invoke(this);
    }
    public void DeselectSpell()
    {
        _panelCanvasGroup.alpha = 0.5f;
        _abilityInfoTransform.DOScale(0.7f, 0.15f);
    }
    private void OnDestroy()
    {
        PlayerInputController.ChangedOnGamepad -= OnChangedToGamepad;
        _player.ManaChanged -= OnManaAmountChanged;
        _player.FreeSpellCounterChanged -= OnFreeSpellCounterChanged;
        button.onClick.RemoveListener(SelectSpell);
    }
}
