using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;


    public Image fadeToBlackScreen;
    public float fadeSpeed = 1f;
    private bool fadingToBlack, fadingFromBlack;

    public Slider healthSlider;

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

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        FadingController(); // Controla el fadingtoblack o fadingFromBlack.
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
                fadingToBlack = false;  //Cambiamos el controlador boleano para que no haga más cambios.
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
                fadingFromBlack = false;  //Cambiamos el controlador boleano para que no haga más cambios.
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
}
