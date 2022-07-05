using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] [Range(0, 5)] public float volumeMultiplier;

    [Header("Clips")]
    [SerializeField] AudioClip themeMenu;
    [SerializeField] AudioClip themeField; //se modificara para cada stage de normal, agua y fuego, ya depende otras areas dependiendo el tiempo...
    [SerializeField] AudioClip themeCave;
    [SerializeField] AudioClip themeCastle;
    [SerializeField] AudioClip themeTower;
    [SerializeField] AudioClip themeHell;
    [SerializeField] AudioClip themeBoss;
    [SerializeField] AudioClip themeItem;
    [SerializeField] AudioClip themeDead;

    static MusicPlayer instance;

    ThemeName currentTheme;

    public static MusicPlayer Instance
    {
        get { return instance;  }
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
        {
            //ESTO ES LO MISMO QUE ESTA ABAJO

            //if (themeName == ThemeName.None) //tenemos que hacer esto para cada tema.
            //{
            //    audioSource.clip = null;
            //    audioSource.Stop(); 
            //}
            //if (themeName == ThemeName.Menu) 
            //    audioSource.clip = themeMenu;

            //if (themeName == ThemeName.Fields)
            //    audioSource.clip = themeField;

            //if (themeName == ThemeName.Cave)
            //    audioSource.clip = themeCave;

            //if (themeName == ThemeName.Castle)
            //    audioSource.clip = themeCastle;

            //if (themeName == ThemeName.Tower)
            //    audioSource.clip = themeTower;
        } //Abrir

        if (currentTheme == themeName)
            return;

        currentTheme = themeName;
        switch (themeName)
        {
            case ThemeName.None: audioSource.clip = null; audioSource.Stop(); break;
            case ThemeName.Menu: audioSource.clip = themeMenu; break;
            case ThemeName.Fields: audioSource.clip = themeField; break;
            case ThemeName.Cave: audioSource.clip = themeCave; break;
            case ThemeName.Castle: audioSource.clip = themeCastle; break;
            case ThemeName.Tower: audioSource.clip = themeTower; break;
            case ThemeName.Hell: audioSource.clip = themeHell; break;
            case ThemeName.Boss: audioSource.clip = themeBoss; break;
            case ThemeName.Item: audioSource.clip = themeItem; break;
            case ThemeName.Dead: audioSource.clip = themeDead; break;
        }
        audioSource.Play();
        audioSource.volume = volumeMultiplier;
    }

    void Update()
    {
        
    }

    public enum ThemeName
    {
        None,
        Menu,
        Fields,
        Cave,
        Castle,
        Tower,
        Hell,
        Boss,
        Item,
        Dead
    }

    //Se agrega para bajar el volumen en menú de pausa.
    public void ChangeVolume(bool isPaused)
    {
        if (isPaused)
        {
            audioSource.volume = volumeMultiplier / 4;
            audioSource.pitch = .99f;
        }
        else
        {
            audioSource.volume = volumeMultiplier;
            audioSource.pitch = 1f;
        }
    }

}
