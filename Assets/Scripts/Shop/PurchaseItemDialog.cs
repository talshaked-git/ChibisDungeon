using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseItemDialog : MonoBehaviour
{
    public TMP_Text text;
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string message)
    {
        text.text = message;
    }

}
