using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    [SerializeField]
    Image colorImage;
    
    private void Start()
    {
        colorImage.color = GameManager.Instance.Settings.PlayerColors[0];
    }
    public void NextColor()
    {
        colorImage.color = GameManager.Instance.Settings.PlayerColors
            [(GameManager.Instance.Settings.PlayerColors.IndexOf(colorImage.color) + 1) % GameManager.Instance.Settings.PlayerColors.Count];
    }
    public void PreviousColor()
    {
        colorImage.color = (GameManager.Instance.Settings.PlayerColors.IndexOf(colorImage.color) - 1 < 0) ?
                            GameManager.Instance.Settings.PlayerColors[^1] :
                            GameManager.Instance.Settings.PlayerColors[GameManager.Instance.Settings.PlayerColors.IndexOf(colorImage.color) - 1];
    }
}
