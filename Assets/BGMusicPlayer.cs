using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource bgMusicSource;
    [SerializeField][Range(0, 5)] public float volumeMultiplier;

    [Header("Clips")]
    [SerializeField] AudioClip AmbienceMusic01;
    [SerializeField] AudioClip AmbienceMusic02;
    [SerializeField] AudioClip AmbienceMusic03;

    static BGMusicPlayer instance;

    ThemeName currentTheme;

    public static BGMusicPlayer Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        int objectCount = FindObjectsOfType<MusicPlayer>().Length;

        if (objectCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayTheme(ThemeName themeName)
    {

        if (currentTheme == themeName)
            return;

        currentTheme = themeName;
        switch (themeName)
        {
            case ThemeName.None: bgMusicSource.clip = null; bgMusicSource.Stop(); break;
            case ThemeName.AmbienceMusic01: bgMusicSource.clip = AmbienceMusic01; break;
            case ThemeName.AmbienceMusic02: bgMusicSource.clip = AmbienceMusic02; break;
            case ThemeName.AmbienceMusic03: bgMusicSource.clip = AmbienceMusic03; break;
        }
        bgMusicSource.Play();
        bgMusicSource.volume = volumeMultiplier;
    }

    public enum ThemeName
    {
        None,
        AmbienceMusic01,
        AmbienceMusic02,
        AmbienceMusic03
    }

    //Se agrega para bajar el volumen en menú de pausa.
    public void ChangeVolume(bool isPaused)
    {
        if (isPaused)
        {
            bgMusicSource.volume = volumeMultiplier / 4;
            bgMusicSource.pitch = .99f;
        }
        else
        {
            bgMusicSource.volume = volumeMultiplier;
            bgMusicSource.pitch = 1f;
        }
    }

}
