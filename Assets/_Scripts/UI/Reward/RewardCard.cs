using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
   
   public Image Icon;
   private TextMeshProUGUI Name;
   public RewardBase Reward;
   RewardWindow rewardWindow;
   SimpleTooltip tooltip;

    [SerializeField]
    private Image _boarder;




    private void Awake()
    {
        Name = GetComponentInChildren<TextMeshProUGUI>();
        rewardWindow = GetComponentInParent<RewardWindow>();
        tooltip = gameObject.AddComponent<SimpleTooltip>();
        
    }

    public void SetInfo()
    {
        Icon.sprite = Reward.Icon;
        Name.text = Reward.Name;
        tooltip.infoLeft = Reward.GetRewardInfo()[0];
        tooltip.infoRight = Reward.GetRewardInfo()[1];
        
    }


    public void OnSelectRewardCard()
    {
        rewardWindow.SelectCard(this);
    }

    public void SetAsSelected()
    {
        _boarder.color = Color.green;
    }
    public void SetAsDeselected()
    {
        _boarder.color = Color.white;
    }

}
