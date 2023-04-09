using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image icon;
    [SerializeField] Image background;


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
        }
        else
        {
            // Double-click detected
            Debug.Log("Double Click");
            if (item != null && OnPressEvent != null)
            {
                OnPressEvent(item);
            }
        }

        _lastClickTime = currentTime;
    }


    // public void OnPointerClick(PointerEventData eventData)
    // {

    //     // If another item is clicked, reset the double-click state
    //     if (_lastClickedSlot != null && _lastClickedSlot != this)
    //     {
    //         _lastClickedSlot._isWaitingForDoubleClick = false;
    //         _lastClickedSlot.StopAllCoroutines();
    //     }

    //     _lastClickedSlot = this;

    //     if (!_isWaitingForDoubleClick)
    //     {
    //         _isWaitingForDoubleClick = true;
    //         StartCoroutine(DoubleClickDetection());
    //     }
    //     else
    //     {
    //         // Double-click detected
    //         Debug.Log("Double Click");
    //         _isWaitingForDoubleClick = false;
    //         StopAllCoroutines();
    //         if (item != null && OnPressEvent != null)
    //         {
    //             Debug.Log("Item is not null");
    //             OnPressEvent(item);
    //         }
    //     }
    //     if (_isWaitingForDoubleClick) // Add this condition to ensure single-click is triggered only when _isWaitingForDoubleClick is still true
    //     {
    //         Debug.Log(name + " Game Object Clicked!");
    //         _isWaitingForDoubleClick = false;
    //     }


    // }

    // private IEnumerator DoubleClickDetection()
    // {
    //     yield return new WaitForSeconds(_maxDoubleClickTime);
    //     // Single-click detected
    //     if (_isWaitingForDoubleClick) // Add this condition to ensure single-click is triggered only when _isWaitingForDoubleClick is still true
    //     {
    //         Debug.Log(name + " Game Object Clicked!");
    //         _isWaitingForDoubleClick = false;
    //     }
    // }

    protected virtual void OnValidate() {
        if (background == null) {
            background = GetComponent<Image>();
        }
        if (icon == null) {
            icon = transform.Find("Icon").GetComponent<Image>();
        }
    }

}
