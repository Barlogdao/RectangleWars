using UnityEngine;
using DG.Tweening;

public class MagicWall : AbilityBase
{
    [SerializeField]
    private DamageArea _wallPrefab;
    [SerializeField]
    private float _wallSpeed, _distance;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private int _bonusPerLvl;
    [SerializeField] SpriteFrontSide _spriteFrontSide;

    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_damage).ToString() }; ;
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        var wall = Instantiate(_wallPrefab, player.BattlefieldHero.transform.position, Quaternion.identity);
        wall.Init(_damage, player.tag);
        wall.transform.LookAtTarget(player.SpawnerTransform.position, _spriteFrontSide);
        
        wall.GetComponent<SpriteRenderer>().color = player.PlayerColor;
        wall.transform.DOMove(wall.transform.position +(player.SpawnerTransform.up * (_distance)),1f * _wallSpeed).SetSpeedBased().OnComplete(()=> Destroy(wall.gameObject));
        base.UseAbility(battlefieldManager, player);
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        LayerMask mask = player.gameObject.layer == 11 ? LayerMask.GetMask("EnemyUnits") : LayerMask.GetMask("AllyUnits");
        return Physics2D.CircleCast(player.SpawnerTransform.position, 2f, player.SpawnerTransform.up, _distance - 1.5f,mask).collider != null;
    }
}
    
    
