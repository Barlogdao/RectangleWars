using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ResMines : StrategicObjectBase
{
    [SerializeField]
    StrategicObjectsSO strategicObjectData;
    [SerializeField]
    MineVFX _mineVFX;
    [SerializeField]
    private Transform _spawnResPoint;
    [SerializeField]
    private SpriteRenderer _resSR;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsOccupied && other.TryGetComponent(out WorkerClass unit) && unit.CanMove)
        {
            IsOccupied = true;
            unit.SetAsBusy(_type);
            if (unit.IsBusy)
            {
                OccupieObject(unit);
                return;
            }
            IsOccupied = false;
        }
    }
    private void Start()
    {
        _resSR.sprite = strategicObjectData.Resource.Image;
        _resSR.enabled = false;
    }

    protected override void OccupieObject(UnitBase unit)
    {
        base.OccupieObject(unit);
        _mineVFX.VFXON(unit.Owner);
        StartCoroutine(miningRes(unit));
    }

    protected override void FreeObject(UnitBase unit)
    {
        StopAllCoroutines();
        _mineVFX.VFXOFF();
        base.FreeObject(unit);
    }

    //Запускает генерацию ресурса владельцу юнита
    IEnumerator miningRes(UnitBase worker)
    {
        yield return Utilis.GetWait(strategicObjectData.Cooldown);
        while(worker != null && worker.IsAlive)
        {
            UpdateOutlineColor(worker);
            worker.Owner.GetRes(strategicObjectData.resourseType);
            if (worker.Owner is HumanPlayer)
            {
                EarningResVisual();
            }
            yield return Utilis.GetWait(strategicObjectData.Cooldown);
        }
        IsOccupied = false;
    }
    private void EarningResVisual()
    {
        _resSR.transform.position = _spawnResPoint.position;
        _resSR.enabled = true;
        _resSR.DOFade(0f, 0.6f).SetEase(Ease.InCubic).OnComplete(() => _resSR.DOFade(1f, 0f));
        _resSR.transform.DOMoveY(1f, 0.6f).SetRelative(true). OnComplete(() => _resSR.enabled = false); 

    }


}
