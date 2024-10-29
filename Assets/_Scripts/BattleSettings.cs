using Redcode.Extensions;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    [SerializeField] private ArrowRotateType _humanArrow;
    [SerializeField] private ArrowRotateType _enemyArrow;
    public ArrowRotateType HumanArrow => _humanArrow;
    public ArrowRotateType EnemyArrow => _enemyArrow;
    public Transform HumanSpawnPositions;
    public Transform EnemySpawnPositions;
    public Transform HumanStartPoint;
    public Transform EnemyStartPoint;

    public Transform GetNewHeroPosition(BattlefieldHero hero,Transform heroSpawnposions)
    {
        if (heroSpawnposions.childCount >1) 
        {
            Transform newPos = heroSpawnposions.GetRandomChild();
            while(newPos.position == hero.transform.position)
            {
                newPos = heroSpawnposions.GetRandomChild();
            }
            return newPos;
            
        }
        else
        {
            return null;
        }
    }
    public void SetSpawnerColor(Color color, Transform spawnerpoints)
    {
        foreach(Transform spawnerpoint in spawnerpoints)
        {
            spawnerpoint.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
