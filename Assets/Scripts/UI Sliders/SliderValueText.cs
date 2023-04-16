
using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        slider.maxValue = 1;
    }

    public void UpdateText(string textValue)
    {
        text.text = textValue;
    }

    public void UpdateSlider(float value)
    {
        slider.value = value;
    }


    private void OnValidate()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        if (text == null)
        {
            text = slider.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
    }


}
