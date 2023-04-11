using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour, IPointerClickHandler,  IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] public Image icon;
    [SerializeField] Image background;


    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    public bool isTooltipActive = false;



    private Color noramlColor = Color.white;
    private Color disabledColor = new Color(1,1,1,0);

    private Item _item;
    public Item item{
        get { return _item; }
        set {
            _item = value;
            if (_item == null ) {
                icon.color = disabledColor;
            } else {
                icon.color = noramlColor;
                icon.sprite = _item.Icon;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnPressEvent != null)
        {
            OnPressEvent(this);
        }
    }

    protected virtual void OnValidate() {
        if (background == null) {
            background = GetComponent<Image>();
        }
        if (icon == null) {
            icon = transform.Find("Icon").GetComponent<Image>();
        }
    }

    public virtual bool CanReciveItem(Item item) {
        return true;
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null)
        {
            OnBeginDragEvent(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(OnEndDragEvent != null)
        {
            OnEndDragEvent(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragEvent != null)
        {
            OnDragEvent(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(OnDropEvent != null)
        {
            OnDropEvent(this);
        }
    }
}
