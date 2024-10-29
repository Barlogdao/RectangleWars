using UnityEngine;
using Redcode.Extensions;

public class PoisonCloudSpell : AbilityBase
{
    [SerializeField] private PoisonCloud _poisonCloudPrefab;
    [SerializeField] private int _damage;
    [SerializeField] private float _duration;
    [SerializeField] private LayerMask _layerMask;
    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        Vector3 freePosition;
        do
        {
            freePosition = battlefieldManager.MManager.AvaliablePositions.GetRandomElement();
        } while (Physics2D.OverlapBox(freePosition,_poisonCloudPrefab.BoxCollider2D.size,0f,_layerMask) != null);

        Instantiate<PoisonCloud>(_poisonCloudPrefab, freePosition, Quaternion.identity).Init(player, _damage, _duration);
    }
    public override string[] GetParams(Hero hero)
    {
        return new string[] { _damage.ToString(), _duration.ToString() };
    }


    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return Utilis.Chanse(10);
    }

}
