using UnityEngine;

namespace RB.Services.Audio
{
    public class SoundPlayer
    {
        private readonly AudioSource _audioSource;
        private readonly string _volumeTag;
        private readonly string _generalVolumeTag;
        private readonly AudioService _audioService;
        public SoundPlayer(string volumeTag, string generalVolumeTag, AudioService audioService, AudioSource audioSource)
        {
            _volumeTag = volumeTag;
            _generalVolumeTag = generalVolumeTag;
            _audioService = audioService;
            _audioSource = audioSource;
            UpdateVolumeLevel();
        }
        public void SetVolumeLevel(float value)
        {
            PlayerPrefs.SetFloat(_volumeTag, value);
            _audioSource.volume = value * PlayerPrefs.GetFloat(_generalVolumeTag, 1f) * MuteModifier();
        }
        public void UpdateVolumeLevel()
        {
            _audioSource.volume = PlayerPrefs.GetFloat(_volumeTag, 1f) * PlayerPrefs.GetFloat(_generalVolumeTag, 1f) * MuteModifier();
        }

        public void PlaySound(AudioClip clip) => _audioSource.PlayOneShot(clip);

        private int MuteModifier()
        {
            return _audioService.IsMuted ? 0 : 1;
        }
    } 
}
