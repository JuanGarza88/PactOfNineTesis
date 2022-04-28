using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int id;

    BoxCollider2D boxCollider; //


    PlayerData playerData;
    PlayerMovement playerMovement;
    public void Initialize()
    {
        boxCollider = GetComponent<BoxCollider2D>(); //
        playerData = FindObjectOfType<PlayerData>();
        //playerMovement = GetComponent<PlayerMovement>();

        switch (itemName)
        {
            case "Health Upgrade" when playerData.healthUpgrades[id]: Destroy(gameObject); break;
            case "Ammo Upgrade" when playerData.ammoUpgrades[id]: Destroy(gameObject); break;
            case "Weapon Melee" when playerData.weaponMeleeUpgrades[id]: Destroy(gameObject); break;
            case "Weapon Range" when playerData.weaponRangeUpgrades[id]: Destroy(gameObject); break;
            case "Key" when playerData.keys[id]: Destroy(gameObject); break;
        }

        //ES LO MISMO QUE ARRIBA\\
        //if (itemName == "Health Upgrade")
        //{
        //    if(playerData.healthUpgrades[id])
        //        Destroy(gameObject);
                        
        //}
        //if (itemName == "Weapon Melee")
        //{
        //    if (playerData.weaponMeleeUpgrades[id])
        //        Destroy(gameObject);
        //}
        //if (itemName == "Key")
        //{
        //    if (playerData.keys[id])
        //        Destroy(gameObject);
        //}
        //----------------------------------FALTA-------------\\
        //if (itemName == "Dash Upgrade")
        //{
        //    if(playerData)
        //        Destroy(gameObject);

        //}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        playerData = FindObjectOfType<PlayerData>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        switch (itemName)
        {
            case "Health Drop": playerData.Heal(); break;
            case "Ammo Drop": playerData.AddAmmo(); break;
            case "Key" when id == 0: playerData.UpdateUpgrades(itemName, id); UnlockBlocks("Green"); break;
            default: playerData.UpdateUpgrades(itemName, id); break;
        }



        //if (itemName == "Health Drop")
        //{
        //    playerData.Heal();
        //}
        if(itemName == "Dash Upgrade")
        {
            playerData.powerUpDash = true;
            Destroy(gameObject);
        }
        if(itemName == "Jump Upgrade")
        {
            Debug.Log("FuncionaJump");
            playerMovement.jumpsAllowed++;
            Destroy(gameObject);
        }
        if (itemName == "Fire Upgrade")
        {
            Debug.Log("TengoFire");
            playerData.firePower = true;
            playerData.switchPower = 1;
            Destroy(gameObject);
        }
        if (itemName == "Water Upgrade")
        {
            Debug.Log("TengoWater");
            playerData.waterPower = true;
            playerData.switchPower = 2;
            Destroy(gameObject);
        }

        //else if (itemName == "Ammo Drop")
        //{

        //}

        //else //ESTE ES EL DEFAULT DE ARRIBA 
        //{
        //    playerData.UpdateUpgrades(itemName, id);
        //}

        //
        /*if(itemName == "Key")*/ //Se duplica cuando vayamos a utilizar otras llaves para abrir otros bloques. 
        //{
        //    if (id == 0) //si el Id es 0 va a desbloquear todos los bloques que tenga el ID 0.
        //    {
        //        UnlockBlocks("Green");
        //    }
        //}


        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Fountain);
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
