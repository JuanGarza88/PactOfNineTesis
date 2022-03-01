using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    public void LoadData() 
    {

    }

    public void SaveData()
    {
        Debug.Log("SAVE");
    }
}
