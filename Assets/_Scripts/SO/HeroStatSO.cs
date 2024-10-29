using UnityEngine;
using Assets.SimpleLocalization;
using RB.HeroStats;

[CreateAssetMenu(fileName = "New HeroStat", menuName = "ScriptableObjects/HeroStat")]
public class HeroStatSO : ScriptableObject
{
    public HeroStat Stat;
    [SerializeField]
    private string nameKey;
    public Sprite Image;
    public Color StatColor;
    private const string FULL_INFO = "Message.FullInfo";

    public string Name { get => LocalizationManager.Localize(nameKey); }
    public string Description { get => LocalizationManager.Localize(nameKey + LocalizationManager.Desc )+"\n" + LocalizationManager.Localize(FULL_INFO); }
    public string ShortDesc { get => LocalizationManager.Localize(nameKey + LocalizationManager.Desc); }

    public HeroStatEffect[] StatEffects = new HeroStatEffect[5];

}
