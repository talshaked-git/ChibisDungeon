using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;


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
        musicSource = BackgroundMusicSourceManager.instance.backgroundMusicSource;
        sfxSource = SFXMusicSourceManager.instance.sfxMusicSource;
    }

    public void PlayMusic(int clip)
    {
        musicSource.clip = musicClips[clip];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(int clip)
    {
        sfxSource.clip = sfxClips[clip];
        sfxSource.loop = false;
        sfxSource.Play();
    }
}
