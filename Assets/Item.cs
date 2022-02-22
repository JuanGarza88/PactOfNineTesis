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
        if (itemName == "Key")
        {
            if (playerData.keys[id])
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
        if(itemName == "Key")
        {
            if (id == 0) //si el Id es 0 va a desbloquear todos los bloques que tenga el ID 0.
            {
                UnlockBlocks("Green");
            }
        }

        Destroy(gameObject);
    }
    private void UnlockBlocks(string color)
    {
        List<Block> colorBlocks = Blocks(color);
        foreach (var block in colorBlocks)
            block.Unlock();
    }



    List<Block> Blocks(string color)
    {
        Block[] allBlocks = FindObjectsOfType<Block>();
        List<Block> blocks = new List<Block> { };
        foreach(var block in allBlocks)
        {
            if (block.id == color)
                blocks.Add(block);
        }
        return blocks;
    }




}
