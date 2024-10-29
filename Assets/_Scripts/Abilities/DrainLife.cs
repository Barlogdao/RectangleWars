using UnityEngine;
using DG.Tweening;

public class DrainLife : AbilityBase
{

    [SerializeField] private int _damageAmount;
    [SerializeField] private int _healAmount;
    [SerializeField] private float _durationVFX;


    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        
        var enemy = battlefieldManager.GetEnemyBH(player);
        enemy.GetTrueDamage(_damageAmount);
        var particle = Instantiate(m_ParticleSystem, enemy.transform.position, Quaternion.identity);
        particle.transform.DOMove(player.BattlefieldHero.transform.position, _durationVFX).SetEase(Ease.InOutCubic).
            OnComplete(() => {
                Destroy(particle.gameObject);
                player.BattlefieldHero.Health += _healAmount;
            });
        base.UseAbility(battlefieldManager, player);
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return player.BattlefieldHero.Health < player.BattlefieldHero.MaxHealth && Utilis.Chanse(70);
    }

    public override string[] GetParams(Hero hero)
    {
        return new string[] { _damageAmount.ToString(), _healAmount.ToString() };
    }

}
