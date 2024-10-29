
using UnityEngine;
using DG.Tweening;


public class Fortifies : AbilityBase
{
    [SerializeField]
    private int _duration = 3;
    [SerializeField]
    GameObject fortGameObject;

    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_duration + hero.Sorcery).ToString() };
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {

        LayerMask mask = player.gameObject.layer == 11 ? LayerMask.GetMask("EnemyUnits") : LayerMask.GetMask("AllyUnits");
        var unit = Physics2D.OverlapCircle(player.BattlefieldHero.transform.position, 7f, mask);
        return unit != null && IsLookinToHero(player.BattlefieldHero.transform.position, unit.attachedRigidbody) && unit.TryGetComponent(out UnitBase a) && a.IsAlive;

    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        var fort = Instantiate(fortGameObject, player.BattlefieldHero.transform.position, Quaternion.identity);
        fort.tag = player.tag;
        
        fort.GetComponent<SpriteRenderer>().color = player.PlayerColor;
        Instantiate(m_ParticleSystem, fort.transform.position, Quaternion.identity);
        fort.transform.DOScale(0.1f, 0.5f).From();
        Destroy(fort, _duration + player.Hero.Sorcery);
        base.UseAbility(battlefieldManager, player); ;
    }

    private bool IsLookinToHero(Vector2 heroPosition, Rigidbody2D enemy)
    {
        return Vector2.Dot((Vector2)enemy.transform.position - heroPosition, enemy.velocity) < 0;
    }
}
