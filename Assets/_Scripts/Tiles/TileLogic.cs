using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    
    public TileSO TileData;
    [SerializeField]
    ParticleSystem m_ParticleSystem;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        UnitBase target = collision.GetComponent<UnitBase>();

        if (TileData.IsChangeSpeed)
        { 
            if (TileData.SpeedRatio > 1f)
                target.Speed *= TileData.SpeedRatio;
                
            else if (NotImmuneToTile(target))
            {
                target.Speed *= TileData.SpeedRatio;
            }            
        }

        if (TileData.IsStopUnit && Utilis.Chanse(TileData.ChanseToStop) && NotImmuneToTile(target))
        {
            InstantiateParticle(target);
            target.ImmobilizeUnit(TileData.StopTime);
        }
        if (TileData.IsDealDamage && Utilis.Chanse(TileData.ChanseToDealDamage) && NotImmuneToTile(target))
        {
            InstantiateParticle(target);
            StartCoroutine(MakeDamage(target,TileData.Damage));
        }
        if (TileData.IsHeal)
        {
            if (target.Health < target.MaxHealth)
            {
                InstantiateParticle(target);
            }
            target.Heal(TileData.HealAmount);
        }
        if (TileData.IsKillUnit && Utilis.Chanse(TileData.ChanseToKill) && NotImmuneToTile(target))
        {
            InstantiateParticle(target);
            StartCoroutine(MakeDamage(target, target.Health));
        }
        if (TileData.IsChangeDirection && Utilis.Chanse(TileData.ChanseToChangeDirection) && NotImmuneToTile(target))
        {
            InstantiateParticle(target);
            target.RandomMove();
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        UnitBase target = collision.GetComponent<UnitBase>();

        if (TileData.IsChangeSpeed)
        {
            if (TileData.SpeedRatio > 1f)
                target.Speed /= TileData.SpeedRatio;
            else if (NotImmuneToTile(target))
            {
                target.Speed /= TileData.SpeedRatio;
            }            
        }
    }

    private IEnumerator MakeDamage(UnitBase target,int damage)
    {
        yield return Utilis.GetWait(0.2f);
        if (target != null && target.IsAlive)
        {
            target.GetTrueDamage(damage);            
        }
       
    }
    private void InstantiateParticle(UnitBase target)
    {
        if (m_ParticleSystem != null)
            Instantiate(m_ParticleSystem, target.transform.position.AddY(0.5f), Quaternion.identity, target.transform);
    }
    private bool NotImmuneToTile(UnitBase target)
    {
        
        if (target.PerkBaseList.Exists(x => 
        x is PerkWalkType walkType && 
        (walkType.walkType == TileData.ImmuneWalkType || walkType.walkType == WalkType.Landwalk)))
        {
            return false;
        }
       
       return true;        
    }
}
