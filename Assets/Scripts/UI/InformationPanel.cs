using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    public void SetInfoText(string text)
    {
        infoText.text = text;
    }

}
