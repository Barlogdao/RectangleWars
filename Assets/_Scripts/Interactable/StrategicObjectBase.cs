using UnityEngine;


public enum SOType
{
    None = 0,
    Resmine = 1,
    PowerPlace = 2,
    BattleRestrictedObject = 3
}
[RequireComponent(typeof(SpriteRenderer))]
public abstract class StrategicObjectBase : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected Transform _standpoint;
    [SerializeField]
    protected SOType _type;
    public SOType Type => _type;
    protected bool _isOccupied = false;
    protected const string IS_ACTIVE = "IsActive";

    public bool IsOccupied
    {
        get { return _isOccupied; }
        set
        {
            _isOccupied = value;
            gameObject.layer = IsOccupied ? 3 : 7;
            OnOccupiedStateChange();
        }
    }



    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_OutlineThickness", 1.5f);
        OutlineOFF();
        OnAwake();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnOccupiedStateChange() { }


    protected void OutlineON(UnitBase unit)
    {
        spriteRenderer.material.SetFloat("_OutlineValue", 2f);
        spriteRenderer.material.SetColor("_OutlineColor", unit.Owner.PlayerColor);
    }
    protected void OutlineOFF()
    {
        spriteRenderer.material.SetFloat("_OutlineValue", 0f);
    }

    protected void SetUnitAtObject(Transform unitTransform)
    {
        unitTransform.position = _standpoint.transform.position;
    }

    protected virtual void OccupieObject(UnitBase unit)
    {
        IsOccupied = true;
        OutlineON(unit);
        unit.LocalDieEvent += FreeObject;
        unit.NotBusy += FreeObject;
        SetUnitAtObject(unit.transform);
    }

    protected virtual void FreeObject(UnitBase unit)
    {
        unit.LocalDieEvent -= FreeObject;
        unit.NotBusy -= FreeObject;
        OutlineOFF();
        IsOccupied = false;
    }
    protected void UpdateOutlineColor(UnitBase unit)
    {
        spriteRenderer.material.SetColor("_OutlineColor", unit.Owner.PlayerColor);
    }


}
