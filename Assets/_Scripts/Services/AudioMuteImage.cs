using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RB.Services.Audio
{
    [RequireComponent(typeof(Image))]
    public class AudioMuteImage : MonoBehaviour, IPointerClickHandler
    {
        private Image _image;
        [SerializeField] private Sprite _muteOnImage;
        [SerializeField] private Sprite _muteOffImage;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _image.sprite = AudioService.Instance.IsMuted?_muteOnImage:_muteOffImage;
            AudioService.MuteStateChanged += OnMuteStateChanged;
        }

        private void OnMuteStateChanged(bool isMuted)
        {
            _image.sprite = isMuted ? _muteOnImage : _muteOffImage;
        }

        private void OnDisable()
        {
            AudioService.MuteStateChanged -= OnMuteStateChanged;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioService.Instance.ToggleMuteAudio();
        }
    } 
}
