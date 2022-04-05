using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//SCENE MANAGMENT\\
public class MainMenu : MonoBehaviour
{
    [SerializeField] Text buttonText;

    private void Start()
    {
        MusicPlayer.Instance.PlayTheme(MusicPlayer.ThemeName.Menu);

        string saveString = PlayerPrefs.GetString("Player Data 1", "0|000|000000000000|00000");
        bool noData = saveString[0].ToString() == "0";
        if (noData)
            buttonText.text = "New Game";
        else
            buttonText.text = "Load Game";
    }

    public void GoToSaveFiles()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Save Files Menu");
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Beep2); //Puede cambiar el sonido moviendole al final.
    }

    public void GoToCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Beep2);
    }

}
