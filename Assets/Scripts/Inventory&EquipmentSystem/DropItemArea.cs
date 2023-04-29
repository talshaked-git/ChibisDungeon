using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DropItemArea : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public event Action OnDropEvent;

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null)
        {
            OnDropEvent();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerUIManager.instance.ResumeGame();
    }

    public void Show()
    {
        OnDropEvent = null;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
