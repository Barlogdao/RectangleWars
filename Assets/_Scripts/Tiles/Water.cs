using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Water : MonoBehaviour
{
    [SerializeField]
    ParticleSystem m_ParticleSystem;
    
    TilemapCollider2D m_Tilemap;

    private void Start()
    {
        m_Tilemap = GetComponent<TilemapCollider2D>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitBase>(out UnitBase target) 
            //&& target.PerksWalktype.
            && NotImmuneToTile(target))
        {
            target.StartCoroutine(StartEffect(target,collision));
        }
    }
    IEnumerator StartEffect(UnitBase target, Collider2D collision)
    {
        target.Speed *= 0.4f;
        Instantiate(m_ParticleSystem, target.transform.position, Quaternion.identity);
        while (collision.IsTouching(m_Tilemap))
        {
            target.GetTrueDamage(3);
            yield return Utilis.GetWait(2f);
            Instantiate(m_ParticleSystem, target.transform.position, Quaternion.identity);
        }
        target.Speed /= 0.4f;
    }
    private bool NotImmuneToTile(UnitBase target)
    {
		if (target.PerkBaseList.Exists(x =>
        x is PerkWalkType walkType &&
        (walkType.walkType == WalkType.Waterwalk || walkType.walkType == WalkType.Landwalk)))
        {
            return false;
        }
        return true;
    }
}

