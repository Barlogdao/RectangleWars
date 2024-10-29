using System;
using UnityEngine;
using UnityEngine.UI;

public class TabInfoWindowController : MonoBehaviour
{
    [SerializeField] RectTransform _window;

    private Button _tabButton;
    public event Action<TabInfoWindowController> TabClicked;

    private void Awake()
    {
        _tabButton = GetComponent<Button>();

    }
    private void Start()
    {
        _tabButton.onClick.AddListener(() => TabClicked?.Invoke(this));
    
    }
    public void Show()
    {
        _window.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _window.gameObject.SetActive(false);
    }

}
