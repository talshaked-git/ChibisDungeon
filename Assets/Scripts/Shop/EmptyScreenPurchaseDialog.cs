using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyScreenPurchaseDialog : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<PurchaseItemDialog>().Hide();
    }
}

