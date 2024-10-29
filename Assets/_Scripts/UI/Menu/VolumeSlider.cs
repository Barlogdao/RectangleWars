using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    EventFloatSO volumeEvent;
    Slider slider;

    private void Awake()
    {
       slider = GetComponent<Slider>();
       slider.value = PlayerPrefs.GetFloat(volumeEvent.PrefsTag, 1f);
       
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat(volumeEvent.PrefsTag, 1f);
        slider.onValueChanged.AddListener(volumeEvent.RaiseEvent);
    }
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(volumeEvent.RaiseEvent);
    }

}
