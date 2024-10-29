using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitStatBar : MonoBehaviour
{
    [SerializeField]
    UnitStatsSO stat;
    [SerializeField]
    Image statIcon;
    [SerializeField]
    TextMeshProUGUI label, amount;

    public void SetStat(string statAmount)
    {
        statIcon.sprite = stat.Image;
        if (label != null)
        {
            label.text = stat.Name;
        }
        
        amount.text = statAmount;
    }


}
