using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    GameManager gameManager;
    PlayerData playerData;

    private bool initialized; 

    public void Initialize()
    {
        if (!initialized)
        {
            initialized = true; //Para que lo haga una vez

            gameManager = FindObjectOfType<GameManager>();
            switch (GameManager.Instance.useTestValues)
            {
                case true: LoadTestData(); break;
                case false: LoadData(); break;
            }
            
        }
    }

    public void LoadData() 
    {
        //Si ponemos mas cosas para guardar tenemos que aregar mas strings o sea cadena de texto\\
        gameManager = FindObjectOfType<GameManager>();
        playerData = FindObjectOfType<PlayerData>();

        string saveString =  PlayerPrefs.GetString(gameManager.SaveSlotKey(), "0|000|000000000000|00000"); //cadena de texto
        Debug.Log("Loading Data: " + saveString);

        string[] saveStrings = saveString.Split('|');

        playerData.checkpoint = int.Parse(saveStrings[0]);
        playerData.weaponMeleeUpgrades = StringToBoolArray(saveStrings[1]);
        playerData.healthUpgrades = StringToBoolArray(saveStrings[2]);
        playerData.keys = StringToBoolArray(saveStrings[3]);

        playerData.UpdateStats();
        //esto se puede hacer mas sencillo. La parte "gameManager.enterPoint = 2;" puede cambiar del 2 al 1 ya que es
        if(playerData.checkpoint == 0)
        {
            gameManager.enterPoint = 0;
            SceneManager.LoadScene("Stage 01x");
        }
        if (playerData.checkpoint == 1)
        {
            gameManager.enterPoint = 2; //2 //que tenga el element 2\\ // O Que sea 0 ya que es el default.
            SceneManager.LoadScene("Stage 01x");
        }
        if (playerData.checkpoint == 2)  //CHECAR al terminar\\ 
        {
            gameManager.enterPoint = 2; //2 //que tenga el element 2\\ //
            SceneManager.LoadScene("Stage 03x");
        }
    }

    private void LoadTestData()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerData = GetComponent<PlayerData>();
        var testValues = GetComponent<TestValues>();

        GameManager.Instance.enterPoint = 0;
        playerData.weaponMeleeUpgrades = testValues.weaponMeleeUpgrades;
        playerData.healthUpgrades = testValues.healthUpgrades;
        playerData.keys = testValues.keys;

        playerData.UpdateStats();

        initialized = true;

        FindObjectOfType<StageManager>().InitializeStage();
    }

    public void SaveData()
    {
        string saveString = ""; //"0|000|000000000000|00000"
        saveString += playerData.checkpoint + "|";

        foreach (bool upgrade in playerData.weaponMeleeUpgrades)
            saveString += upgrade ? "1" : "0"; //cada vez que agarremos un powerMelee si es vdd se pone un 1 y si no el 0.

        saveString +=  "|";

        foreach (bool upgrade in playerData.healthUpgrades)
            saveString += upgrade ? "1" : "0";

        saveString +=  "|";

        foreach (bool key in playerData.keys)
            saveString += key ? "1" : "0";

        PlayerPrefs.SetString(gameManager.SaveSlotKey(), saveString);

        Debug.Log("Saving Data: " + saveString);
    }

    bool[] StringToBoolArray(string intString)
    {
        bool[] boolArray = new bool[intString.Length];
        for (var i = 0; i < intString.Length; i++)
        {
            boolArray[i] = intString[i].ToString() == "1"; //Si el string es = a 1 lo guardara como verdadero
        }
        return boolArray;
    }
}
