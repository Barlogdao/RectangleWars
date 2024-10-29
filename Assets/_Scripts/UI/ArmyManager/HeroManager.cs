using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;
using System;
using System.Linq;

public class HeroManager : MonoBehaviour
{
    [SerializeField]
    private Image heroImage, armyFrame, spellFrame;
    private Color _armyFrameColor, _spellFrameColor;
    [SerializeField]
    private TextMeshProUGUI HeroName, Leadership, Sorcery, Stamina;
    [SerializeField] private HeroStatPoints _leadershipPoints, _sorceryPoints, _staminaPoints;
    [SerializeField]
    private Transform armyPanel, spellPanel, InventoryPanel;
    private List<UnitDataSO> unitList;
    private List<SpellSO> spellList;
    private HeroInventory playerInventory;
    private Hero hero;
    [SerializeField]
    private HeroArmyInformation _enemyArmy;
    [SerializeField] private InsightToolTip _insight;


    private SlotItemBase[] armySlotItemsList;
    private SlotItemBase[] spellSlotItemsList;
    private SlotItemBase[] inventorySlotItemsList;
    public Action<SlotItemBase> ItemStartDrag;
    public Action<SlotItemBase> ItemEndDrag;
    public Action<SlotItemBase> ItemDoubleClicked;
    private List<ArmySlot> _armySlots;


    [SerializeField] private Image _blocker;
    [SerializeField] private HeroStatDescriptionWindow _heroStatDescriptionWindow;
    [SerializeField] private UnitFullInfoTip _unitFullInfo;
    [SerializeField] private SpellFullInfoTip _spellFullInfo;


    void Start()
    {
        hero = GameManager.Instance.Hero;
        unitList = new List<UnitDataSO>(hero.StartUnit);
        spellList = new List<SpellSO>(hero.StartSpell);
        playerInventory = new HeroInventory(hero.Inventory);
        _armyFrameColor = armyFrame.color;
        _spellFrameColor = spellFrame.color;


        armySlotItemsList = armyPanel.GetComponentsInChildren<SlotItemBase>();
        spellSlotItemsList = spellPanel.GetComponentsInChildren<SlotItemBase>();
        inventorySlotItemsList = InventoryPanel.GetComponentsInChildren<SlotItemBase>();


        heroImage.sprite = hero.HeroSprite;
        HeroName.text = LocalizationManager.Localize(hero.HeroNameKey);
        Leadership.text = $"{LocalizationManager.Localize("Hero.Leadership")}: "; ;
        Sorcery.text = $"{LocalizationManager.Localize("Hero.Sorcery")}: ";
        Stamina.text = $"{LocalizationManager.Localize("Hero.Stamina")}: ";

        _leadershipPoints.SetPoints(hero.Leadership);
        _sorceryPoints.SetPoints(hero.Sorcery);
        _staminaPoints.SetPoints(hero.Stamina);

        _armySlots = armyPanel.GetComponentsInChildren<ArmySlot>().ToList();

        InitSpellPanel();
        InitArmyPanel();
        InitInventory();
        _enemyArmy.Init(GameManager.Instance.EnemyHero);
        _insight.Init(hero, hero.Insights[0], hero.Insights[0].Image);

        Instantiate(_unitFullInfo, transform.root);
        Instantiate(_spellFullInfo, transform.root);

    }

    private void HeroStatTooltip_HeroStatClicked(HeroStatSO stat)
    {
        _blocker.gameObject.SetActive(true);
        _heroStatDescriptionWindow.ShowHeroStat(stat, hero);
    }

    void InitArmyPanel()
    {
        foreach (var armyslot in _armySlots)
        {
            armyslot.SetUnitInSlot(unitList);
        }
    }
    void InitSpellPanel()
    {
        foreach (var slot in spellSlotItemsList)
        {
            slot.ItemImage.enabled = false;
        }

        for (int i = 0; i < spellList.Count; i++)
        {
            spellSlotItemsList[i].Spell = spellList[i];
            spellSlotItemsList[i].ItemImage.enabled = true;
            spellSlotItemsList[i].ItemImage.sprite = spellList[i].Image;
            spellSlotItemsList[i].RefershToolTip();
        }
    }
    void InitInventory()
    {
        foreach (var slot in inventorySlotItemsList)
        {
            slot.ItemImage.enabled = false;
        }
        
        for (int i = 0; i < playerInventory.Units.Count; i++)
        {
            inventorySlotItemsList[i].Unit = playerInventory.Units[i];
            inventorySlotItemsList[i].ItemImage.enabled = true;
            inventorySlotItemsList[i].ItemImage.sprite = playerInventory.Units[i].Image;
            inventorySlotItemsList[i].RefershToolTip();
        }
        for (int i = playerInventory.Units.Count; i < playerInventory.GetLastIndex(); i++)
        {
            inventorySlotItemsList[i].Spell = playerInventory.Spells[i - playerInventory.Units.Count];
            inventorySlotItemsList[i].ItemImage.enabled = true;
            inventorySlotItemsList[i].ItemImage.sprite = playerInventory.Spells[i - playerInventory.Units.Count].Image;
            inventorySlotItemsList[i].RefershToolTip();
        }
    }
    public void NextTurn()
    {
        hero.StartUnit.Clear();
        foreach (var unit in armySlotItemsList)
        {
            if (unit.Unit != null && !unit.Unit.BlankUnit)
                hero.StartUnit.Add(unit.Unit);
        }
        hero.StartSpell.Clear();
        foreach (var spell in spellSlotItemsList)
        {
            if (spell.Spell != null)
                hero.StartSpell.Add(spell.Spell);
        }
        hero.Inventory.Clear();
        foreach (var item in inventorySlotItemsList)
        {
            if (item.Unit != null && !item.Unit.BlankUnit)
                hero.Inventory.Units.Add(item.Unit);
            else if (item.Spell != null)
                hero.Inventory.Spells.Add(item.Spell);
        }

        EventBus.NextLevelEvent.Invoke();
    }
    private void OnEnable()
    {
        ItemStartDrag += OnItemStartDrag;
        ItemEndDrag += OnItemEndDrag;
        ItemDoubleClicked += OnItemDoubleClicked;
        HeroStatTooltip.HeroStatClicked += HeroStatTooltip_HeroStatClicked;
    }
    private void OnDisable()
    {
        ItemStartDrag -= OnItemStartDrag;
        ItemEndDrag -= OnItemEndDrag;
        ItemDoubleClicked -= OnItemDoubleClicked;
        HeroStatTooltip.HeroStatClicked -= HeroStatTooltip_HeroStatClicked;
    }

    void OnItemStartDrag(SlotItemBase item)
    {
        if (item.Unit != null)
            armyFrame.color = Color.green;
        else if (item.Spell != null)
            spellFrame.color = Color.green;
    }
    void OnItemEndDrag(SlotItemBase item)
    {
        armyFrame.color = _armyFrameColor;
        spellFrame.color = _spellFrameColor;
    }

    public void OnItemDoubleClicked(SlotItemBase slotItem)
    {
        // If Unit
        if (slotItem.Unit != null)
        {

            // Slot in Inventory
            if (inventorySlotItemsList.Contains(slotItem))  
            {
                var armySlot = _armySlots.Find(s => s.Class == slotItem.Unit.Class);
                var armySlotItem = armySlot.Item;
                if (!armySlot.IsUnitUnlocked()) return;
                if (armySlotItem.Unit.BlankUnit)
                {
                    armySlotItem.SetDataInSlot(slotItem);
                    slotItem.ClearSlot();
                }
                else if (!armySlotItem.Unit.BlankUnit) 
                {
                    armySlotItem.SwapSlotsData(slotItem);
                }
            }

            // Slot In Panel

            else if (armySlotItemsList.Contains(slotItem) && !slotItem.Unit.BlankUnit)
            {
                if (inventorySlotItemsList.Any(slot => slot.IsEmpty))
                {
                    foreach (var slot in inventorySlotItemsList)
                    {
                        if (slot.IsEmpty)
                        {
                            var armyslot = _armySlots.Find(s => s.Class == slotItem.Unit.Class);
                            slot.SetDataInSlot(slotItem);
                            armyslot.SetStandartUnit();
                            break;
                        }
                    }
                }
            }
        }
        // If Spell
        else if (slotItem.Spell != null)
        {
            // Slot in Inventory
            if (inventorySlotItemsList.Contains(slotItem))
            {
                foreach (SlotItemBase spellSlot in spellSlotItemsList)
                {
                    if (spellSlot.IsEmpty)
                    {
                        spellSlot.SwapSlotsData(slotItem);
                    }
                }

            }
            // Slot In Panel
            if (spellSlotItemsList.Contains(slotItem))
            {
                foreach (var slot in inventorySlotItemsList)
                {
                    if (slot.IsEmpty)
                    {
                        slot.SwapSlotsData(slotItem);
                        break;
                    }
                }
            }


        }
    }

}
