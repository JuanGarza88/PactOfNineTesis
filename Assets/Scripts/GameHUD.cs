using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{

    [SerializeField] Text weaponText;

    [SerializeField] Image[] hearts;//, heartsImages;  //contener imagenes de los corazones.
    [SerializeField] Sprite hearthEmpty, heartFull;

    [SerializeField] Image[] keysImages;

    PlayerData playerData;

    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        UpdateHealth();
    }

    void Update()
    {
        UpdateHealth();


        if (playerData.UpgradeCount(playerData.weaponMeleeUpgrades) == 0)
            weaponText.text = "--";
        else
            weaponText.text = "x" + playerData.UpgradeCount(playerData.weaponMeleeUpgrades);
        for (int i = 0; i < playerData.keys.Length; i++)
        {
            if (!playerData.keys[i])
                keysImages[i].color = Color.black; //O sea que se mueste que hay un item por desbloquear.
            else
                keysImages[i].color = Color.white;
        }
    }

    private void UpdateHealth()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = new Color(1, 1, 1, 1); //255\\ o = Color.white; //No es que cambie el color se queda el grafico con su diseño nrml.
            if (i < playerData.healthPoints) //Si este corazon esta debajo de los puntos de vida actuales, tons si tengo e cora.
                hearts[i].sprite = heartFull;
            else
                hearts[i].sprite = hearthEmpty;
            if (i > playerData.healthPointsMax - 1)
                hearts[i].color = Color.clear; //Es el color inverso de white. donde los valores esta en 0,0,0,0

            //Debug.Log(i);
        }
    }
}
