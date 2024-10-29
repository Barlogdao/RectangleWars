using UnityEngine;
using UnityEngine.UI;

public class HeroStatPoints : MonoBehaviour
{
    [SerializeField] Image[] _points = new Image[5];
    [SerializeField] private Color _statColor;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = Color.gray;
        foreach (var point in _points)
        {
            point.color = _defaultColor; 
        }
    }
    public void SetPoints(int statLevel)
    {
        ClearPoints();
        for (int i = 0; i < statLevel; i++)
        {
            _points[i].color = _statColor;
        }
    }

    private void ClearPoints()
    {
        foreach (var point in _points)
        {
            point.color = _defaultColor; ;
        }
    }
}
