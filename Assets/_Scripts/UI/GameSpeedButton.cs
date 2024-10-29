using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSpeedButton : MonoBehaviour
{
    [SerializeField]
    private float[] _speeds = new float[4];
    [SerializeField]
    private Sprite[] _speedIcons = new Sprite[4];
    private Image _image;
    private UICanvas _uiCanvas;
    private int _currentIndex = 1;
    private void Awake()
    {
        _image= GetComponent<Image>();
        _image.sprite = _speedIcons[1];
        _uiCanvas = GetComponentInParent<UICanvas>();
    }
    private void Start()
    {
        PlayerInputController.GameSpeedPressed += ChangeGameSpeed;
    }


    public void ChangeGameSpeed()
    {
        if (_uiCanvas.PauseWindow.gameObject.activeInHierarchy) return;
        _currentIndex  = (_currentIndex + 1) % _speeds.Length;
        _uiCanvas.GameSpeed = _currentIndex;
        _image.sprite = _speedIcons[_currentIndex];
        Time.timeScale = _currentIndex;
    }
    private void OnDestroy()
    {
        PlayerInputController.GameSpeedPressed -= ChangeGameSpeed;
    }

}
