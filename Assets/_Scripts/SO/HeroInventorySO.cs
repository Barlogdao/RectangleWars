
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "FolderName/SOINVENORY")]
public class HeroInventorySO : ScriptableObject
{
    [SerializeField] private List<SpellSO> _spellList;
    public List<SpellSO> GetInventorySpells()
    {
        return _spellList;
    }

}
