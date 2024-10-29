using UnityEngine;

namespace RB.Services.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {

        private AudioSource _audioSource;
        private string _volumeTag;
        private string _generalVolumeTag;
        private AudioService _audioService;


        public void Awake()
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

