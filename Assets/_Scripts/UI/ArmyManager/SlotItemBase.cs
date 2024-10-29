using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItemBase : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    public Image ItemImage;
    public UnitDataSO Unit;
    public SpellSO Spell;
    private RectTransform rt;
    private Canvas canvas;
    SimpleTooltip tooltip;
    [SerializeField]
    Canvas ownCanvas;
    private CanvasGroup canvasGroup;
    public bool IsEmpty => Unit == null && Spell == null;

    protected void Awake()
    {
        
        rt = GetComponent<RectTransform>();
        canvas =GetComponentInParent<HeroManager>().GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        ItemImage = GetComponent<Image>();
        tooltip = GetComponent<SimpleTooltip>();

    }


    public void SetDataInSlot(SlotItemBase other)
    {
        Unit = other.Unit;
        Spell = other.Spell;
        ItemImage.enabled = true;
        ItemImage.sprite = other.ItemImage.sprite;
        RefershToolTip();
    }
    public void ClearSlot()
    {
        Unit = null;
        Spell = null;
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        RefershToolTip();
    }
    public void SwapSlotsData(SlotItemBase other)
    {
        (Unit, other.Unit) = (other.Unit, Unit);
        (Spell, other.Spell) = (other.Spell, Spell);
        (ItemImage.sprite, other.ItemImage.sprite) = (other.ItemImage.sprite, ItemImage.sprite);
        RefershToolTip();
        other.RefershToolTip();
        ItemImage.enabled = ItemImage.sprite != null;
        other.ItemImage.enabled = other.ItemImage.sprite != null;
    }

    public void RefershToolTip()
    {
        tooltip.infoLeft = Unit != null ? Unit.GetLeftInfo(GameManager.Instance.Hero) : Spell != null ?
            Spell.GetLeftInfo(GameManager.Instance.Hero) : "";
        tooltip.infoRight = Unit != null ? Unit.GetRightInfo(GameManager.Instance.Hero) : Spell != null ? Spell.GetRightInfo(GameManager.Instance.Hero) : "";

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ownCanvas.sortingOrder++;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        GetComponentInParent<HeroManager>().ItemStartDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Utilis.GetMousePos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        ownCanvas.sortingOrder--;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        GetComponentInParent<HeroManager>().ItemEndDrag?.Invoke(this);

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Unit != null)
                UnitFullInfoTip.ShowInfo?.Invoke(Unit);
            else if (Spell != null)            
                SpellFullInfoTip.ShowInfo?.Invoke(Spell);
            
        } 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            GetComponentInParent<HeroManager>().ItemDoubleClicked?.Invoke(this);
        }
    }
}
