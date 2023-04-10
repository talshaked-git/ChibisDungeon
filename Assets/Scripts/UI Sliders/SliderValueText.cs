
using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    private void Start() {
        slider.onValueChanged.AddListener( UpdateText);
    }

    public void UpdateText(float value) {
        text.text = slider.value.ToString();
    }


    private void OnValidate() {
        if (slider == null) {
            slider = GetComponent<Slider>();
        }
        if (text == null) {
            text = slider.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
    }

}
