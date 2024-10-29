using System;
using UnityEngine;


namespace RB.Services.Audio
{
    public class AudioService : SingletonPersistent<AudioService>
    {

        [SerializeField] private EventFloatSO _generalVolumeEvent;
        [SerializeField] private EventFloatSO _musicVolumeEvent;
        [SerializeField] private EventFloatSO _soundVolumeEvent;
        public const string MUTE_AUDIO = "MUTE_AUDIO";

        private MusicPlayer _musicPlayer;
        private SoundPlayer _soundPlayer;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        public static event Action<bool> MuteStateChanged;

        [field: SerializeField] public bool FadeOnSwitchMusic { get; private set; }
        [field: SerializeField] public float FadeSwitchingDuration { get; private set; }
        [field: SerializeField] public bool IsMuted { get; private set; }

        protected override void OnAwake()
        {
            LoadMuteValue();
            _musicPlayer = new MusicPlayer(_musicVolumeEvent.PrefsTag, _generalVolumeEvent.PrefsTag, this, _musicSource);
            _soundPlayer = new SoundPlayer(_soundVolumeEvent.PrefsTag, _generalVolumeEvent.PrefsTag, this, _soundSource);
        }

        private void OnEnable()
        {
            _generalVolumeEvent.AddListener(OnGeneralVolumeChanged);
            _musicVolumeEvent.AddListener(OnMusicVolumeChanged);
            _soundVolumeEvent.AddListener(OnSoundVolumeChanged);
        }

        public void PlaySound(AudioClip clip)
        {
            _soundPlayer.PlaySound(clip);
        }
        public void PlayMusic(AudioClip clip, bool playWithoutFade = false)
        {
            _musicPlayer.PlayMusic(clip, playWithoutFade);
        }
        public bool ToggleMuteAudio()
        {
            IsMuted = !IsMuted;
            SaveMuteValue();
            UpdateAudioPlayers();
            MuteStateChanged?.Invoke(IsMuted);
            return IsMuted;
        }

        private void OnSoundVolumeChanged(float volume)
        {
            _soundPlayer.SetVolumeLevel(volume);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            _musicPlayer.SetVolumeLevel(volume);
        }

        private void OnGeneralVolumeChanged(float volume)
        {
            PlayerPrefs.SetFloat(_generalVolumeEvent.PrefsTag, volume);
            UpdateAudioPlayers();
        }


        protected override void OnDisableHandler()
        {
            _generalVolumeEvent.RemoveListener(OnGeneralVolumeChanged);
            _musicVolumeEvent.RemoveListener(OnMusicVolumeChanged);
            _soundVolumeEvent.RemoveListener(OnSoundVolumeChanged); ;
        }

        private void LoadMuteValue()
        {
            int value = PlayerPrefs.GetInt(MUTE_AUDIO, 0);
            IsMuted = value != 0;
        }
        private void SaveMuteValue()
        {
            PlayerPrefs.SetInt(MUTE_AUDIO, IsMuted ? 1 : 0);
        }

        private void UpdateAudioPlayers()
        {
            _musicPlayer.UpdateVolumeLevel();
            _soundPlayer.UpdateVolumeLevel();
        }

    }

    public enum AudioChannels
    {
        General,
        Music,
        Sound,
    }

}