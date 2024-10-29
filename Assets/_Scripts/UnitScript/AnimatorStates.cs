using UnityEngine;

public class AnimatorStates
{
    public static int Idle { get => Animator.StringToHash("Idle"); }
    public static int Walk { get => Animator.StringToHash("Walk"); }
    public static int Attack { get => Animator.StringToHash("Attack"); }
    public static int Die { get => Animator.StringToHash("Die"); }
    public static int Summon { get => Animator.StringToHash("Summon"); }
    public static int Special { get => Animator.StringToHash("Special"); }
}
