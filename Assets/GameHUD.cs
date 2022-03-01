using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] Image[] hearths; //contener imagenes de los corazones.
    [SerializeField] Sprite hearthEmpty, heartFull;

    PlayerData playerData;

    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        UpdateHealth();
    }

    void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for(int i = 0; i < hearths.Length; i++)
        {
            hearths[i].color = new Color(1, 1, 1, 1); //255\\ o = Color.white; //No es que cambie el color se queda el grafico con su diseño nrml.
            if (i < playerData.healthPoints) //Si este corazon esta debajo de los puntos de vida actuales, tons si tengo e cora.
                hearths[i].sprite = heartFull;
            else
                hearths[i].sprite = hearthEmpty;
            if (i > playerData.healtPointsMax - 1)
                hearths[i].color = Color.clear; //Es el color inverso de white. donde los valores esta en 0,0,0,0

            //Debug.Log(i);
        }
    }
}
