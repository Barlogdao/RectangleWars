using UnityEngine;
using Assets.SimpleLocalization;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewUnitStat", menuName = "ScriptableObjects/UnitStat")]
public class UnitStatsSO : ScriptableObject
{
    [SerializeField]
    private string nameKey;
    [ShowAssetPreview]
    public Sprite Image;
    public string Name { get => LocalizationManager.Localize(nameKey); }
    public string Description { get => LocalizationManager.Localize(nameKey + LocalizationManager.Desc); }
}
