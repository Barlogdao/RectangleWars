using UnityEngine;

public static class RwExtensions
{
    public static Vector3 AddY(this Vector3 vector, float offset)
    {
        return new Vector3(vector.x, vector.y + offset, vector.z);
    }


    public static void LookAtTarget(this Transform transform, Vector3 targetPos, SpriteFrontSide spriteFront)
    {
        Vector2 direction = targetPos - transform.position;
        
        switch(spriteFront)
        {
            case SpriteFrontSide.Left: transform.right = -direction;
                break;
            case SpriteFrontSide.Right: transform.right = direction;
                break;
            case SpriteFrontSide.Top: transform.up = direction;
                break;
            case SpriteFrontSide.Bottom: transform.up = -direction;
                break;
            default: break;

        };
    }


}
