using UnityEngine;
using UnityEngine.UI;

public class UnitInfoTableUI : MonoBehaviour
{
    [SerializeField] private UnitDataUI _imagePrefab;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private UnitFullInfoTip _fullUnitInfo;
    


    void Start()
    {
        foreach (var unit in GameLibrary.Instance.Fractions.GetAllUnits())
        {
            var im = Instantiate(_imagePrefab, _contentParent);
            im.Init(unit, ShowFullUnitInfo);
        }
       Instantiate(_fullUnitInfo, transform.root);
    }

    private void ShowFullUnitInfo(UnitDataSO unit)
    {
        UnitFullInfoTip.ShowInfo?.Invoke(unit);
    }


}
