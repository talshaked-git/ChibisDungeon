
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    private float targetValue;
    private float time;

    private void Awake()
    {
        slider.maxValue = 1;
        slider.minValue = 0;
        slider.value = 0;
    }

    public void UpdateText(string textValue)
    {
        text.text = textValue;
    }

    public void UpdateSlider(float value)
    {
        targetValue = value;
        time = 0;
        if (this.isActiveAndEnabled)
            StartCoroutine(AnimateBar());
    }

    private IEnumerator AnimateBar()
    {
        while (slider.value != targetValue)
        {
            time += Time.unscaledDeltaTime;
            slider.value = Mathf.Lerp(slider.value, targetValue, time);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
