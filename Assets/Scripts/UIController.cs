using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;


    public Image fadeToBlackScreen;
    public float fadeSpeed = 1f;
    private bool fadingToBlack, fadingFromBlack;

    public string mainMenuScene;

    public GameObject pauseScreen;
    public bool isPaused;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        FadingController(); // Controla el fadingtoblack o fadingFromBlack.

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    private void FadingController()
    {
        if (fadingToBlack)
        {
            fadeToBlackScreen.color = new Color(fadeToBlackScreen.color.r, //RGB, no se mueve. 
                                                fadeToBlackScreen.color.g,
                                                fadeToBlackScreen.color.b,
                                                Mathf.MoveTowards(fadeToBlackScreen.color.a, 1f, fadeSpeed * Time.deltaTime)); //Solamente cambiamos el Alpha a 1 = Negro.
            if (fadeToBlackScreen.color.a == 1f) //Si la pantalla ya es completamente negra.
            {
                fadingToBlack = false;  //Cambiamos el controlador boleano para que no haga m?s cambios.
            }

        }
        else if (fadingFromBlack)
        {
            fadeToBlackScreen.color = new Color(fadeToBlackScreen.color.r, //RGB, no se mueve. 
                                                fadeToBlackScreen.color.g,
                                                fadeToBlackScreen.color.b,
                                                Mathf.MoveTowards(fadeToBlackScreen.color.a, 0f, fadeSpeed * Time.deltaTime)); //Solamente cambiamos el Alpha a 0 = transparente.
            if (fadeToBlackScreen.color.a == 0f) //Si la pantalla ya es completamente transparente.
            {
                fadingFromBlack = false;  //Cambiamos el controlador boleano para que no haga m?s cambios.
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadingFromBlack = false;
        fadingToBlack = true;
    }

    public void StartFadeFromBlack()
    {
        fadingFromBlack = true;
        fadingToBlack = false;

    }

    public void PauseUnpause()
    {

        if(!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            isPaused = true;
            MusicPlayer.Instance.ChangeVolume(true);
        }
        else
        {
            pauseScreen.SetActive(false);
            isPaused = false;
            MusicPlayer.Instance.ChangeVolume(false);
        }
    }

    public void GoToMainMenu() // se crea una funcion y se le da la referencia de la escena del menu principal
    {
        Time.timeScale = 1f;
        instance = null;
        Destroy(gameObject);
        SceneManager.LoadScene(mainMenuScene);
    }

    public void PlaySFX()
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.MenuSelect);
    }


}
