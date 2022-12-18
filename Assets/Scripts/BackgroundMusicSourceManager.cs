using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicSourceManager : MonoBehaviour
{
    public static BackgroundMusicSourceManager instance;
    public AudioSource backgroundMusicSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            backgroundMusicSource = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        backgroundMusicSource.volume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
    }
}
