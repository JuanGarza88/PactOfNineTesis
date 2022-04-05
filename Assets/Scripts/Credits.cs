using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private void Start() //este metdo se copia en todas las pistas que quieres que se reproduzca dicha cancion.
    {
        MusicPlayer.Instance.PlayTheme(MusicPlayer.ThemeName.Menu);
    }
    public void BackToMainMenu()
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Beep2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
