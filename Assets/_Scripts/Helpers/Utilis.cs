using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteFrontSide { Left, Right, Top, Bottom }
public static class Utilis
{
    public static bool Chanse(int percent)
    {
        return (float)percent/100 >= Random.value;
    }
    public static bool Chanse(float percent)
    {
        return percent >= Random.Range(0f,100f);
    }
    private static readonly Dictionary<float,WaitForSeconds> WaitDictionary = new Dictionary<float,WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }
    public static Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
    
    public static Quaternion GetRotationforTarget(Transform transform, Vector3 targetPos, SpriteFrontSide spriteFront)
    {
        Vector2 direction = targetPos - transform.position;
        float angle = spriteFront switch
        {
           SpriteFrontSide.Left => Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg,
           SpriteFrontSide.Top => Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg,
           SpriteFrontSide.Right => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
           SpriteFrontSide.Bottom => Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg,
             _ => 0
        };
        return Quaternion.AngleAxis(angle, Vector3.forward);
        
    }

    public static float GetParticleSpawnYPos(UnitBase unit, SpawnPosition position)
    {
        switch (position)
        {
            case SpawnPosition.Bottom: return 0f;
            case SpawnPosition.Center: return unit.SpriteHeight / 2;
            case SpawnPosition.Top: return unit.SpriteHeight - 0.1f;
        }
        return 0f;
    }
}
