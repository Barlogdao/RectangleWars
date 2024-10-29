using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using Coffee.UIEffects;

public class UnitInfo : MonoBehaviour
{
    private UnitDataSO _unitData;
    private KeyCode _useKey;
    private ArmyPanel _armyPanel;
    public UnitDataSO UnitData => _unitData;
    Button button;
    private HumanPlayer _player;
    public event Action<UnitInfo> UnitInfoSelected;



    [Header("����� ��������")]
    [SerializeField] TextMeshProUGUI goldCost;

    [SerializeField]
    TextMeshProUGUI keyDisplay;
    [Header("����������� �����")]
    [SerializeField] Image unitImage;
    [SerializeField] Image frame;
    [Header("����� �����")]
    [SerializeField] Color enableColor;
    [SerializeField] Color disableColor;
    [SerializeField]
    RectTransform _unitSlot;
    [SerializeField]
    CanvasGroup _unitSlotCanvasGroup;

    [SerializeField] UIShiny _frameShine;
 
    
    private void Awake()
    {
        button = GetComponent<Button>();
        _armyPanel = GetComponentInParent<ArmyPanel>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(_useKey))
        {
            SelectUnit();
            _armyPanel.OnSummonUnitPressed();
        }
    }
    public void InitUnitBar(KeyCode key, UnitDataSO unitData, HumanPlayer player)
    {
        _player = player;
        _unitData = unitData;
        //unitImage.sprite = _unitData.Image;
        unitImage.sprite = player.Hero.StartUnit.Contains(unitData)?
            _unitData.Image
            : GameLibrary.Instance.Fractions.GetClassConfig(unitData.Class).ClassIcon;
        goldCost.text = _unitData.GoldCost.ToString();
        _player.OnStartCD += Recharge;        
        button.onClick.AddListener(SelectUnit);
        _player.GoldChanged += OnGoldChanged;
        _useKey = key;
        keyDisplay.text = _useKey.ToString();
        PlayerInputController.ChangedOnGamepad += OnChangedToGamepad;
    }

    private void OnChangedToGamepad(bool value)
    {
        keyDisplay.text = value == true ? "" : _useKey.ToString();
    }

    private void OnGoldChanged(int goldAmount)
    {
        frame.color = ResCheck()? enableColor:disableColor;
        _frameShine.enabled = frame.color == enableColor;
    }


    // ������� ��� ��������� ������� �� ������
    public void ShowUnit()
    {
        EventBus.HoverObjectData?.Invoke(UnitData);
    }
    // ������� ��� ������ ������� �� ������
    public void ExitUnit()
    {
        EventBus.ExitObjectData?.Invoke();
    }

    // ����� ����������� ������
    private void Recharge(float cd)
    {
        frame.fillAmount = 0f;
        unitImage.fillAmount = 0f;
        unitImage.color = Color.gray;
        frame.DOFillAmount(1, cd);
        unitImage.DOFillAmount(1, cd -0.02f).OnComplete(()=>
        {
            unitImage.color = Color.white;
            transform.DOScale(1.1f, 0.1f).From();
        });
    }
        
    // �������� ���������� �� �������� � ������ �� �����
    private bool ResCheck()
    {
        return _player.Gold >= _unitData.GoldCost;
    }
    private void OnDestroy()
    {
        PlayerInputController.ChangedOnGamepad -= OnChangedToGamepad;
        button.onClick.RemoveListener(SelectUnit);
        if (_player == null) return;
        _player.OnStartCD -= Recharge;
        _player.GoldChanged -= OnGoldChanged;
    }

    public void SelectUnit()
    {
        _unitSlotCanvasGroup.alpha = 1f;
        _unitSlot.DOScale(1.15f, 0.15f);
        EventBus.NewDataObjectSelected?.Invoke(UnitData);
        UnitInfoSelected?.Invoke(this);
    }
    public void DeselectUnit()
    {
        _unitSlotCanvasGroup.alpha = 0.5f;
        _unitSlot.DOScale(0.7f, 0.15f);
    }

}
