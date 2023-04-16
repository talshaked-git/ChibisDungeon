using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] public Image icon;
    [SerializeField] Image background;
    [SerializeField] TMP_Text amountText;


    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    public bool isTooltipActive = false;



    private Color noramlColor = Color.white;
    private Color disabledColor = new Color(1, 1, 1, 0);

    private Item _item;
    public virtual Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null)
            {
                EquipmentSlot equipmentSlot = this as EquipmentSlot;
                if (equipmentSlot != null)
                {
                    icon.sprite = equipmentSlot.defualtIcon;
                }
                else
                {
                    icon.color = disabledColor;
                }
            }
            else
            {
                icon.color = noramlColor;
                icon.sprite = _item.Icon;
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            if (_amount < 0) _amount = 0;
            if (_amount == 0) item = null;
            if (amountText != null)
            {
                amountText.enabled = _item != null && _amount > 1;
                if (amountText.enabled)
                {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    public virtual bool CanAddStack(Item item, int amount = 1)
    {
        return this.item != null && this.item.ID == item.ID && Amount + amount <= item.MaxStack;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPressEvent != null)
        {
            OnPressEvent(this);
        }
    }

    protected virtual void OnValidate()
    {
        if (background == null)
        {
            background = GetComponent<Image>();
        }
        if (icon == null)
        {
            icon = transform.Find("Icon").GetComponent<Image>();
        }
        if (amountText == null)
        {
            amountText = GetComponentInChildren<TMP_Text>();
        }
    }

    public virtual bool CanReciveItem(Item item)
    {
        return true;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragEvent != null)
        {
            OnBeginDragEvent(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragEvent != null)
        {
            OnEndDragEvent(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragEvent != null)
        {
            OnDragEvent(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null)
        {
            OnDropEvent(this);
        }
    }
}
