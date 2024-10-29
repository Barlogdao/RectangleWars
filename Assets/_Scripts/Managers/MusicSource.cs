
using System.Collections;
using UnityEngine;

namespace RB.Services.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSource : MonoBehaviour
    {
        private AudioSource _audioSource;
        private string _volumeTag;
        private string _generalVolumeTag;
        private AudioService _audioService;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(string volumeTag, string generalVolumeTag, AudioService audioService)
        {
            _volumeTag = volumeTag;
            _generalVolumeTag = generalVolumeTag;
            _audioService = audioService;
        }
        private void Start()
        {
            _audioSource.volume = PlayerPrefs.GetFloat(_volumeTag, 1f) * PlayerPrefs.GetFloat(_generalVolumeTag, 1f) * MuteModifier();
        }

        public void PlayMusic(AudioClip clip, bool playWithoutFade = false)
        {
            if (_audioSource.clip == clip) return;
            if (_audioService.FadeOnSwitchMusic && playWithoutFade == false)
            {
                StartCoroutine(FadingSwitchMusic(_audioService.FadeSwitchingDuration, clip));
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
            float start = PlayerPrefs.GetFloat(_volumeTag, 1f);
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
