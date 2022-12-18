using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        musicSource = BackgroundMusicSourceManager.instance.backgroundMusicSource;
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

    public void onValChangeBGMusic(float val)
    {
        musicSource.volume = val;
    }
}
