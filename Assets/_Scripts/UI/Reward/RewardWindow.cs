using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RewardWindow : MonoBehaviour
{
    //SpawnUnit
    UnitDataSO rewardUnit;
    //GetSpell
    SpellSO rewardSpell;
    [SerializeField]
    RewardCard unitCard, spellCard, statCard;
    [SerializeField]
    Transform Header;
    public Button Btn;
    private bool _isAddRewardPerformed = false;
    private RewardCard _selectedRewardCard;

    private void Start()
    {
        Btn = GetComponentInChildren<Button>();
        Btn.gameObject.SetActive(false);
        GetComponentInParent<UICanvas>().EndLevelWindow.gameObject.SetActive(false);
        rewardUnit = GameLibrary.Instance.Fractions.GetAvaliableUnit(GameManager.Instance.Hero);
        rewardSpell = GameLibrary.Instance.Fractions.GetAvaliableSpell(GameManager.Instance.Hero);
        unitCard.Reward = rewardUnit != null ? new RewardUnit(rewardUnit) : new RewardHeroStat();
        unitCard.SetInfo();
        spellCard.Reward = rewardSpell != null ? new RewardSpell(rewardSpell) : new RewardHeroStat();
        spellCard.SetInfo();
        statCard.Reward = new RewardHeroStat();
        statCard.SetInfo();

        Header.gameObject.SetActive(false);
        unitCard.transform.DOMoveY(-80f, 0.4f).SetUpdate(true).From();
        spellCard.transform.DOMoveY(-100f, 0.45f).SetUpdate(true).From();
        statCard.transform.DOMoveY(-120f, 0.5f).SetUpdate(true).From().OnComplete(() => Header.gameObject.SetActive(true));
        unitCard.GetComponent<Selectable>().Select();
    }
    public void SelectCard(RewardCard card)
    {
        if (_selectedRewardCard != null)
            _selectedRewardCard.SetAsDeselected();
        _selectedRewardCard = card;
        _selectedRewardCard.SetAsSelected();
        if(!Btn.gameObject.activeInHierarchy) Btn.gameObject.SetActive(true);
    }

    public void GoToHeroManager()
    {
        if (_selectedRewardCard == null) return;
        _selectedRewardCard.Reward.Execute();
        if (Utilis.Chanse(GameManager.Instance.Settings.AdditionalRewardChanse) && !_isAddRewardPerformed)
        {
            _isAddRewardPerformed = true;
            unitCard.gameObject.SetActive(false);
            statCard.gameObject.SetActive(false);
            spellCard.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.gameObject);

            AdditionalReward();
        }
        else
        {
            DOTween.KillAll();


            EventBus.GoToArmyManagerEvent.Invoke();
        }
    }

    private void AdditionalReward()
    {
        _selectedRewardCard = null;
        rewardUnit = GameLibrary.Instance.Fractions.GetAvaliableUnit(GameManager.Instance.Hero);
        rewardSpell = GameLibrary.Instance.Fractions.GetAvaliableSpell(GameManager.Instance.Hero);
        spellCard.gameObject.SetActive(true);
        Btn.gameObject.SetActive(false);
        Header.gameObject.SetActive(false);
        spellCard.SetAsDeselected();
        if (Utilis.Chanse(50))
        {
            spellCard.Reward = rewardSpell != null ? new RewardSpell(rewardSpell) : new RewardHeroStat();
        }
        else
        {
            spellCard.Reward = rewardUnit != null ? new RewardUnit(rewardUnit) : new RewardHeroStat();
        }
        spellCard.SetInfo();

        spellCard.transform.DOScale(0f, 0.8f).From().SetEase(Ease.OutBounce).SetUpdate(true);
        spellCard.GetComponent<Selectable>().Select();

    }
}
