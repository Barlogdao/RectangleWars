using System.Collections;
using UnityEngine;

namespace RB.Services.Audio
{
    public class MusicPlayer
	{
        private readonly string _volumeTag;
        private readonly string _generalVolumeTag;
        private readonly AudioService _audioService;
        private readonly AudioSource _audioSource;

        public MusicPlayer (string volumeTag, string generalVolumeTag, AudioService audioService, AudioSource audioSource)
        {
            _volumeTag = volumeTag;
            _generalVolumeTag = generalVolumeTag;
            _audioService = audioService;
            _audioSource = audioSource;
            UpdateVolumeLevel();
        }
        public void PlayMusic(AudioClip clip, bool playWithoutFade = false)
        {
            if (_audioSource.clip == clip) return;
            if (_audioService.FadeOnSwitchMusic && playWithoutFade == false)
            {
                _audioService.StartCoroutine(FadingSwitchMusic(_audioService.FadeSwitchingDuration, clip));
            }
            else
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }
        public void PauseMusic() => _audioSource.Pause();
        public void UnpauseMusic() => _audioSource.UnPause();


        private IEnumerator FadingSwitchMusic(float duration, AudioClip clip)
        {
            float currentTime = 0;
            float start = PlayerPrefs.GetFloat(_volumeTag, 1f) * MuteModifier();
            while (currentTime < duration / 2)
            {
                currentTime += Time.unscaledDeltaTime;
                _audioSource.volume = Mathf.Lerp(start, 0f, currentTime / (duration / 2));
                yield return null;
            }
            _audioSource.clip = clip;
            _audioSource.Play();
            currentTime = 0;
            while (currentTime < duration / 2)
            {
                currentTime += Time.unscaledDeltaTime;
                _audioSource.volume = Mathf.Lerp(0f, start, currentTime / (duration / 2));
                yield return null;
            }
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

        private int MuteModifier()
        {
            return _audioService.IsMuted ? 0 : 1;
        }
    } 
}
