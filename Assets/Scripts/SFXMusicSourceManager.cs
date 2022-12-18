using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMusicSourceManager : MonoBehaviour
{
    public static SFXMusicSourceManager instance;
    public AudioSource sfxMusicSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        sfxMusicSource = GameObject.FindGameObjectWithTag("SFXMusic").GetComponent<AudioSource>();
    }
}
