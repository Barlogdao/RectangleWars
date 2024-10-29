using UnityEngine;

public class MediumPerk : PerkBase
{
    [SerializeField] private int _manaAmount;
    public override void UsePerk(IDamagable another, UnitBase self)
    {
        if (!self.IsAlive)
        {
            self.Owner.Mana += _manaAmount;
            Instantiate(VfxProvider.Instance.ManaGainEffect,new Vector3(self.transform.position.x, self.transform.position.y + self.SpriteHeight/2, self.transform.position.z),Quaternion.identity, self.transform);
        }
        
    }
    public override string[] GetParams()
    {
        return new string[] {_manaAmount.ToString()};
    }
}
