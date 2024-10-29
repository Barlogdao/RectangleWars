using UnityEngine;
using UnityEngine.UI;

public class UnitClassDisplay : MonoBehaviour
{
    [SerializeField] private Image _classImage;
    [SerializeField] private ClassSO _classConfig;
    private void Start()
    {
        _classImage.sprite = _classConfig.ClassIcon;
    }



}
