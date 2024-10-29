using UnityEngine;


public abstract class RewardBase
{
    public Sprite Icon;
    public string Name;


    public abstract void Execute();
    public abstract string[] GetRewardInfo();
 }
