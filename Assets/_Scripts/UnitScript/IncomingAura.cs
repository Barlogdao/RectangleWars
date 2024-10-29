using System;
using UnityEngine;

public class IncomingAura : MonoBehaviour
{
    public event Action<UnitBase> UnitEnterAura;
    public event Action<UnitBase> UnitLeaveAura;
    
    public void SetAuraSize(AuraSize size)
    {
        switch (size)
        {
            case AuraSize.Small: GetComponent<CircleCollider2D>().radius = GameLibrary.Instance.SmallAura;
                break;
            case AuraSize.Medium: GetComponent<CircleCollider2D>().radius = GameLibrary.Instance.MediumAura;
                break;
            case AuraSize.Large: GetComponent<CircleCollider2D>().radius = GameLibrary.Instance.LargeAura;
                break;
            case AuraSize.Zero: GetComponent<CircleCollider2D>().radius = 0f;
                break;
        }
    }

    public float GetAuraSize()
    {
        return GetComponent<CircleCollider2D>().radius;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.TryGetComponent(out UnitBase unit))
       {
            UnitEnterAura?.Invoke(unit);
       } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitBase unit))
        {
            UnitLeaveAura?.Invoke(unit);
        }
    }
    private void OnDestroy()
    {
        UnitEnterAura = null;
        UnitLeaveAura = null;
    }

}
