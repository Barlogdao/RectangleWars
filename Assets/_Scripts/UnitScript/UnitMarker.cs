using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMarker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color allyColor;
    public Color enemyColor;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
