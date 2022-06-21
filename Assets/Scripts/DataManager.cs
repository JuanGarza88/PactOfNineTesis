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

        //checkpoint | melee | health/Hearts | keys\\ OLD
        //current health | current ammo | checkpoint | extra health/Hearts | extra ammo | melee | range | keys | Fire | Water | Dash | ExtJump\\ New 
        /////////0|||||||||||||||1||||||||||||2|||||||||||||||||3||||||||||||||||4||||||||||5|||||||6|||||||7||||8|||||||9|||||10|||||11|||
        ///////////////falta poner powerups de dash,doubleJump,fire,water.\\\\\\\\\\\\Preguntar a LordMarco.

        string saveData = PlayerPrefs.GetString(gameManager.SaveSlotKey(), "5|0|0|00000000000|00000000000|000|000|00000|0|0|0|0"); //cadena de texto
        Debug.Log("Loading Data: " + saveData);
        Debug.Log(playerData.gameObject.name,playerData.gameObject);
        Debug.Log("Count" + FindObjectsOfType<PlayerData>().Length);

        string[] saveStrings = saveData.Split('|');
        
        //-----------------------------cosas que se guardan, ponte altiro,tq:)--------------------------------------------\\
        playerData.healthPoints = int.Parse(saveStrings[0]);

        playerData.ammo = int.Parse(saveStrings[1]);

        playerData.checkpoint = int.Parse(saveStrings[2]);

        playerData.healthUpgrades = StringToBoolArray(saveStrings[3]);


        playerData.ammoUpgrades = StringToBoolArray(saveStrings[4]);

        playerData.weaponMeleeUpgrades = StringToBoolArray(saveStrings[5]);

        playerData.weaponRangeUpgrades = StringToBoolArray(saveStrings[6]);

        playerData.keys = StringToBoolArray(saveStrings[7]);
        //______________________Preguntar si se cambia el 0_________________\\
        playerData.firePower = StringToBoolArray(saveStrings[8].ToString())[0];

        playerData.waterPower = StringToBoolArray(saveStrings[9].ToString())[0]; 

        playerData.powerUpDash = StringToBoolArray(saveStrings[10].ToString())[0];

        playerData.extraJump = StringToBoolArray(saveStrings[11].ToString())[0];
        Debug.Log("Extra JumpSave: " + saveStrings[11]);
        Debug.Log("Extra Jump: " + playerData.extraJump);
        playerData.UpdateStats();
        //esto se puede hacer mas sencillo. La parte "gameManager.enterPoint = 2;" puede cambiar del 2 al 1 ya que es 
        /*???? ya que es que?? Juan del pasado. atte: Juan del presente*/

        switch (playerData.checkpoint)//Es lo mismo que los if de abajo pero no los borres por si un dia se te va la onda va y los tomes de ref.
        {
            case 0: gameManager.enterPoint = 0; SceneManager.LoadScene("NS-001"); break;
            case 1: gameManager.enterPoint = 0; SceneManager.LoadScene("NS-007"); break;
        }

        //if(playerData.checkpoint == 0)
        //{
        //    gameManager.enterPoint = 0;
        //    SceneManager.LoadScene("NS-001");
        //}
        //if (playerData.checkpoint == 1)
        //{
        //    gameManager.enterPoint = 0;
        //    SceneManager.LoadScene("NS-007");
        //}

        //if (playerData.checkpoint == 1)
        //{
        //    gameManager.enterPoint = 2; //2 //que tenga el element 2\\ // O Que sea 0 ya que es el default.
        //    SceneManager.LoadScene("Stage 01x");
        //}
        //if (playerData.checkpoint == 2)  //CHECAR al terminar\\ 
        //{
        //    gameManager.enterPoint = 2; //2 //que tenga el element 2\\ //
        //    SceneManager.LoadScene("Stage 03x");
        //}
    }

    private void LoadTestData()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerData = GetComponent<PlayerData>();
        var testValues = GetComponent<TestValues>();

        GameManager.Instance.enterPoint = 0;

        playerData.healthPoints = testValues.healthPoints;

        playerData.ammo = testValues.ammo;

        playerData.weaponMeleeUpgrades = testValues.weaponMeleeUpgrades;

        playerData.weaponRangeUpgrades = testValues.weaponRangeUpgrades;

        playerData.healthUpgrades = testValues.healthUpgrades;

        playerData.ammoUpgrades = testValues.ammoUpgrades;

        playerData.keys = testValues.keys;

        

        playerData.UpdateStats();

        initialized = true;

        FindObjectOfType<StageManager>().InitializeStage();
    }

    public void SaveData()
    {
        //------------Cmpleto-----------------\\
        string saveString = ""; //"0|000|000000000000|00000" //checkpoint | melee | health/Hearts | keys\\ OLD

        //current health | current ammo | checkpoint | extra health/Hearts | ammoUpgrades | melee | range | keys \\ New 
        /////////0|||||||||||||||1||||||||||||2|||||||||||||||||3||||||||||||||||4||||||||||5|||||||6|||||||7||||||||
        ///////////////falta poner powerups de dash,doubleJump,fire,water.\\\\\\\\\\\\Preguntar a LordMarco.
        ///// ahora tiene que estar "5|0|0|00000000000|00000000000|000|000|00000"
        /////current health | current ammo | checkpoint | extra health/Hearts | extra ammo | melee | range | keys | Fire | Water | Dash | ExtJump\\ New 
        /////////0|||||||||||||||1||||||||||||2|||||||||||||||||3||||||||||||||||4||||||||||5|||||||6|||||||7||||8|||||||9|||||10|||||11|||
        ///"5|0|0|00000000000|00000000000|000|000|00000|0|0|0|0"\\\

        saveString += playerData.healthPoints + "|"; //0 new

        saveString += playerData.ammo + "|"; //1 new


        saveString += playerData.checkpoint + "|"; //2 new

        foreach (bool upgrade in playerData.healthUpgrades) // 3new
            saveString += upgrade ? "1" : "0";

        saveString += "|";


        foreach (bool upgrade in playerData.ammoUpgrades) // 4 new
            saveString += upgrade ? "1" : "0"; 

        saveString +=  "|";

        foreach (bool upgrade in playerData.weaponMeleeUpgrades) // 5
            saveString += upgrade ? "1" : "0"; //cada vez que agarremos un powerMelee si es vdd se pone un 1 y si no el 0.

        saveString += "|";

        foreach (bool upgrade in playerData.weaponRangeUpgrades) // 6
            saveString += upgrade ? "1" : "0"; 

        saveString += "|";


        foreach (bool key in playerData.keys) //7
            saveString += key ? "1" : "0";

        saveString += "|";

        saveString += playerData.firePower ? "1" : "0";  //8

        saveString += "|";

        saveString += playerData.waterPower ? "1" : "0"; //9

        saveString += "|";

        saveString += playerData.powerUpDash ? "1" : "0"; //10

        saveString += "|";

        saveString += playerData.extraJump ? "1" : "0"; //11

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
