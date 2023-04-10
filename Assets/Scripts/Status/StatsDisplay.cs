using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text ValueText;

    private void OnValidate() {
        TMP_Text[] text = GetComponentsInChildren<TMP_Text>();
        NameText = text[0];
        ValueText = text[1];
    }
    
}
