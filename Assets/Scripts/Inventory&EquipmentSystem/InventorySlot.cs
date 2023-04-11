using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image icon;
    [SerializeField] Image background;
    [SerializeField] ItemTooltip tooltip;
    public static bool isTooltipActive = false;
    public bool isCurrentTooltipActive = false;
    public event UnityAction<bool,InventorySlot> OnTooltipActiveChanged;

    public event Action<Item> OnPressEvent;

    // private bool _isWaitingForDoubleClick;
    private TimeSpan _maxDoubleClickTime = TimeSpan.FromSeconds(0.7);
    private DateTime _lastClickTime;

    private static InventorySlot _lastClickedSlot;


    private Item _item;
    public Item item{
        get { return _item; }
        set {
            _item = value;
            if (_item == null) {
                icon.enabled = false;
            } else {
                icon.enabled = true;
                icon.sprite = _item.Icon;
            }
        }
    }

public void OnPointerClick(PointerEventData eventData)
    {
        // If another item is clicked, reset the double-click state
        if (_lastClickedSlot != null && _lastClickedSlot != this)
        {
            _lastClickedSlot._lastClickTime = DateTime.MinValue;
        }

        _lastClickedSlot = this;

        DateTime currentTime = DateTime.UtcNow;

        if (currentTime - _lastClickTime > _maxDoubleClickTime)
        {
            // Single-click detected
            Debug.Log(name + " Game Object Clicked!");
            if(item != null && OnPressEvent != null && (isTooltipActive == false || isCurrentTooltipActive == false))
            {
                isTooltipActive = true;
                isCurrentTooltipActive = true;
                if (OnTooltipActiveChanged != null)
                {
                    OnTooltipActiveChanged.Invoke(false, this);
                }
                tooltip.ShowTooltip(item);
            }
            else
            {
                isTooltipActive = false;
                if (OnTooltipActiveChanged != null)
                {
                    OnTooltipActiveChanged.Invoke(false, null);
                }
                tooltip.HideTooltip();
            }
        }
        else
        {
            // Double-click detected
            Debug.Log("Double Click");
            if (item != null && OnPressEvent != null)
            {
                OnPressEvent(item);
                isTooltipActive = false;
                tooltip.HideTooltip();
            }
        }

        _lastClickTime = currentTime;
    }

    protected virtual void OnValidate() {
        if (background == null) {
            background = GetComponent<Image>();
        }
        if (icon == null) {
            icon = transform.Find("Icon").GetComponent<Image>();
        }

        if (tooltip == null) {
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }
}
