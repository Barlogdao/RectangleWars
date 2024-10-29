using System;
using UnityEngine;
using UnityEngine.UI;

namespace RB.Services.Audio
{
    [RequireComponent(typeof(Toggle))]
	public class AudioMuteToggle : MonoBehaviour
	{
		private Toggle _toggle;
        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.isOn = AudioService.Instance.IsMuted;
            _toggle.onValueChanged.AddListener(ToggleMute);
        }

        private void ToggleMute(bool mute)
        {
            AudioService.Instance.ToggleMuteAudio();
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(ToggleMute);
        }

    } 
}
