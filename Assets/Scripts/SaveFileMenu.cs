using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileMenu : MonoBehaviour
{
    [SerializeField] SaveFileTab[] saveTabs;

    GameManager gameManager;
    DataManager dataManager;
    void Start()
    {
        MusicPlayer.Instance.PlayTheme(MusicPlayer.ThemeName.Menu);

        gameManager = FindObjectOfType<GameManager>(); //Le asignamos valor para encontrar el GameManager dentro del juego.
        dataManager = FindObjectOfType<DataManager>();

        //string saveString = "1|110|111000000000|11100"; //"0checkpoint|melee000|<3corazones000000000000|llaves00000"  para testear
        //PlayerPrefs.SetString(FindObjectOfType<GameManager>().SaveSlotKey(), saveString);
        

        string saveString1 = PlayerPrefs.GetString("Player Data 1", "0|000|000000000000|00000");
        string saveString2 = PlayerPrefs.GetString("Player Data 2", "0|000|000000000000|00000");
        string saveString3 = PlayerPrefs.GetString("Player Data 3", "0|000|000000000000|00000");

        saveTabs[0].ShowSaveData(saveString1);
        saveTabs[1].ShowSaveData(saveString2);
        saveTabs[2].ShowSaveData(saveString3);
    }

    public void LoadSaveFile(int saveSlot)
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Beep3);
        gameManager.saveSlot = saveSlot;
        dataManager.LoadData();
    }
    public void BackToMainMenu()
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Beep2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    
}
