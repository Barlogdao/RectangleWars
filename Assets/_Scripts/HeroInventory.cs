using System.Collections.Generic;


[System.Serializable]
public class HeroInventory
{
    public List<UnitDataSO> Units = new List<UnitDataSO>();
    public List<SpellSO> Spells = new List<SpellSO>();
    public void Clear()
    {
        Units.Clear();
        Spells.Clear();
    }
    public HeroInventory(HeroInventory other)
    {
        Units = new List<UnitDataSO>(other.Units);
        Spells = new List<SpellSO>(other.Spells);
    }
    public int GetLastIndex()
    {
        return Units.Count + Spells.Count;
    }
}
