
public class CastlingPerk : PerkBase
{

    public override void UsePerk(IDamagable damageSourse, UnitBase unit)
    {
        if (unit.IsAlive)
        {
            (unit.Attack, unit.Health) = (unit.Health, unit.Attack);
        }
    }
}
