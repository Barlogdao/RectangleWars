using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{   
   
    float startangle;
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float angle;

    [SerializeField] private Transform _arrowVisual;
    private ArrowRotateType _rotationType = ArrowRotateType.FixAngleToMapCenter;
    Tweener tweener;

    private void Awake()
    {
        _arrowVisual.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Init(Player player)
    {   
        LookAtCentre();
        _rotationType = player.ArrowRotateType;
        startangle = _arrowVisual.transform.localEulerAngles.z;
        
        _arrowVisual.transform.Rotate(new Vector3(0, 0, 1), startangle-angle / 2, Space.Self);
        //tweener = _arrowVisual.DOLocalRotate(new Vector3(0, 0, startangle + angle / 2), speed).SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        SetRotation(_rotationType);
        if (player is HumanPlayer)
            _arrowVisual.GetComponent<SpriteRenderer>().enabled = true;
        transform.hasChanged = false;
        void SetRotation(ArrowRotateType type)
        {
            switch (type)
            {
                case ArrowRotateType.FixAngleToMapCenter:
                    tweener = _arrowVisual.DOLocalRotate(new Vector3(0, 0, startangle + angle / 2), speed).SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                    break;
                case ArrowRotateType.Round:
                    tweener = _arrowVisual.DOLocalRotate(new Vector3(0, 0, angle / 10), speed).SetRelative(true).SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
                    break;
            }
        }
    }
    private void Update()
    {
        if (transform.hasChanged)
        {
            LookAtCentre();
            transform.hasChanged = false;
        }
    }


    private void LookAtCentre() => transform.up = Vector3.zero - transform.position;
 
    private void OnDestroy()
    {
        tweener.Kill();
    }


}

public enum ArrowRotateType
{
    FixAngleToMapCenter,
    Round
}

