using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCharacterDialog : MonoBehaviour
{
    public event Action OnYesEvent;
    public event Action OnNoEvent;

    public void Show()
    {
        gameObject.SetActive(true);
        OnYesEvent = null;
        OnNoEvent = null;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnYes()
    {
        if (OnYesEvent != null)
        {
            OnYesEvent();
        }
        Hide();
    }

    public void OnNo()
    {
        if (OnNoEvent != null)
        {
            OnNoEvent();
        }
        Hide();
    }
}
