using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public static SliderController instance;

    [Header("Music Volume Sliders")]
    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SFXSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGMusicVolume", 1f) * 100f;
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f) * 100f;
    }

    public void onValChangeBGMusic()
    {
        AudioManager.instance.musicSource.volume = BGMSlider.value / 100f;
        PlayerPrefs.SetFloat("BGMusicVolume", BGMSlider.value / 100f);
    }

    public void onValChangeSFX()
    {
        AudioManager.instance.sfxSource.volume = SFXSlider.value / 100f;
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value / 100f);
    }
}
