using UnityEngine;
/// <summary>
/// Шанс не получить урона
/// </summary>

public class ConsumingPerk : PerkBase
{
    [SerializeField]
    int chanse = 15;
    [SerializeField]
    AudioClip sound;

    public override void UsePerk(IDamagable damageSourse, UnitBase unit)
    {
        if (damageSourse.Class != ClassType.Hero && Utilis.Chanse(chanse))
        {
            unit.CurrentDamage = 0;
            EventBus.MissSoundEvent?.Invoke();
        }  
    }
    public override string[] GetParams()
    {
        return new string[] { chanse.ToString()};
    }


}
