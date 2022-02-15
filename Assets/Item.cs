using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int id; 

    PlayerData playerData;

    private void Start()
    {
        playerData = FindObjectOfType<PlayerData>();

        if (itemName == "Health Upgrade")
        {
            if(playerData.healthUpgrades[id])
                Destroy(gameObject);
                        
        }
        if (itemName == "Weapon Melee")
        {
            if (playerData.weaponMeleeUpgrades[id])
                Destroy(gameObject);
        }

        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (itemName == "Health Drop")
        {
            playerData.Heal(3);
        }
        else if (itemName == "Ammo Drop")
        {

        }
        else
        {
            playerData.UpdateUpgrades(itemName, id);
        }
        Destroy(gameObject);
    }
}
