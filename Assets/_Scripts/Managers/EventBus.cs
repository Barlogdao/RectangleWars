using UnityEngine;
using System;
using RB.Services.ServiceLocator;

public static class EventBus
{
    // End of Level
    public static Action<BattlefieldHero> GameOverEvent;
    
    

    // UI

    public static Action<ScriptableObject> NewDataObjectSelected;
    public static Action<ScriptableObject> HoverObjectData;
    public static Action ExitObjectData;

    // Damage VFX
    public static Action<Transform> HitUnitEvent;


    public static Action<int, Transform> DamagableHealed;
    public static Action<int, Transform> DamagableDamaged;


    // SOUND
    public static Action<AudioClip> SoundEvent;
    public static Action<AudioClip> HitSoundEvent;
    public static Action<AudioClip> DieSoundEvent;
    public static Action MissSoundEvent;
    public static Action ChangeGeneralVolumeEvent;
    public static Action ButtonSelectSound;

    // LevelTransitions
    public static Action NewGameEvent;
    public static Action ContinueGameEvent;
    public static Action RestartLevelEvent;
    public static Action BackToMainMenuEvent;
    public static Action QuitGameEvent;
    public static Action GoToArmyManagerEvent;
    public static Action NextLevelEvent;
    public static Action RandomGameStart;

}
