using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using RB.Services.Audio;

public class AudioMediator : SingletonPersistent<AudioMediator>
{
    [SerializeField] private AudioService _audioPlayer;

    [Space]
    [Header("Музыка")]
    [SerializeField] private AudioClip[] _menuMusicClips;
    [SerializeField] private AudioClip[] _gameMusicClips;
    [Space]
    [Header("Дефолтные звуки для юнитов")]
    [SerializeField] private AudioClip _unitDamagedClip;
    [SerializeField] private AudioClip _unitDieClip;
    [SerializeField] private AudioClip _missAttackClip;
    [Space]
    [Header("Звуки UI")]
    [SerializeField] private AudioClip _buttonHoverClip;
    [Space]
    [Scene]
    [SerializeField] private int _loadingSceneIndex;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.SoundEvent += PlaySound;
        EventBus.HitSoundEvent += OnUnitDamaged;
        EventBus.DieSoundEvent += OnUnitDie;
        EventBus.ButtonSelectSound += OnButtonHover;
        EventBus.MissSoundEvent += OnAttackMissed;
    }
    #region SoundLogic
    public void PlaySound(AudioClip clip) => _audioPlayer.PlaySound(clip);
    private void OnAttackMissed() => _audioPlayer.PlaySound(_missAttackClip);
    private void OnUnitDamaged(AudioClip clip) => _audioPlayer.PlaySound(clip != null? clip:_unitDamagedClip);
    private void OnUnitDie(AudioClip clip) => _audioPlayer.PlaySound(clip != null ? clip : _unitDieClip);
    private void OnButtonHover() => _audioPlayer.PlaySound(_buttonHoverClip);
    #endregion

    #region MusicLogic
    public void PlayMusic(AudioClip clip)
    {
        _audioPlayer.PlayMusic(clip);
    }
    private void PlayMenuMusic()
    {
        PlayMusic(_menuMusicClips[Random.Range(0, _menuMusicClips.Length)]);
    }

    private void PlayGameMusic()
    {
        PlayMusic(_gameMusicClips[UnityEngine.Random.Range(0, _gameMusicClips.Length)]);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == _loadingSceneIndex) return;

        if (IsMenuScene(scene))
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameMusic();
        }

        bool IsMenuScene(Scene scene)
        {
            return scene.buildIndex == 0 || scene.buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        }
    }
    #endregion

    protected override void OnDisableHandler()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.SoundEvent -= PlaySound;
        EventBus.HitSoundEvent -= OnUnitDamaged;
        EventBus.DieSoundEvent -= OnUnitDie;
        EventBus.ButtonSelectSound -= OnButtonHover;
        EventBus.MissSoundEvent -= OnAttackMissed;
    }
}
