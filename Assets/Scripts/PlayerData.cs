using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] int baseHealthPoints; //Determinamos cuantos son nuestros puntos de vida base.
    [SerializeField] int baseAmmo;
    [SerializeField] int baseMeleeDamage;

    public int checkpoint; //Puede ser un string por si necesitamos una referencia especifica


    public bool[] healthUpgrades;
    public bool[] ammoUpgrades;
    public bool[] weaponMeleeUpgrades;
    public bool[] weaponRangeUpgrades;
    public bool[] keys;

    /// /////////////////////////////////////////////////
    /// POWER UP DASH ACTIVADOR

    public bool powerUpDash; //YA

    public bool firePower = false;
    public bool waterPower;

    public bool extraJump;

    public int switchPower = 0; 
    // 0 normal
    // 1 Fuego
    //2 Agua


    /// //////////////////////////////////////////////////


    public bool greenKeyObtained;
    public bool yellowKeyObtained;
    public bool redKeyObtained;
    public bool purpleKeyObtained;
    public bool whiteKeyObtained;

    public int healthPointsMax;
    public int healthPoints;
    public int ammoMax; 
    public int ammo; 


    public int meleeLvl;
    public int meleeDamage;

    public int rangeLvl;

    //private void Awake()   //Esto ya no\\
    //{
    //    weaponMeleeUpgrades = new bool[] { false, false, false, false }; //Arreflode los upgrdes del melee
    //    healthUpgrades = new bool[] { false, false, false, false, false }; //Arreglo de booleanos 5 items en falso
    //    keys = new bool[] { false, false, false, false, false };
    //}


    public void UpdateStats()
    {

        meleeLvl = GetMaxLvl(weaponMeleeUpgrades); //Estas dos lineas sustituyen a todo lo de abajo
        rangeLvl = GetMaxLvl(weaponRangeUpgrades);


        healthPointsMax = baseHealthPoints + UpgradeCount(healthUpgrades);
        ammoMax = rangeLvl > 0 ? baseAmmo + UpgradeCount(ammoUpgrades) * 5 : 0;

        //Esto es para que haya un orden en las mejoras del arma.
        //rastreamos desde el elemento mas alto y si tiene un valor true asigna lvl 3

       

        //if (weaponMeleeUpgrades[2])
        //    meleeLvl = 3;

        //else if (weaponMeleeUpgrades[1])
        //    meleeLvl = 2;

        //else if (weaponMeleeUpgrades[0])
        //    meleeLvl = 1;

        //else
        //    meleeLvl = 0;

        ////-----------------------------Range----------------------------\\
        //if (weaponRangeUpgrades[2])
        //    rangeLvl = 3;

        //else if (weaponRangeUpgrades[1])
        //    rangeLvl = 2;

        //else if (weaponRangeUpgrades[0])
        //    rangeLvl = 1;

        //else
        //    rangeLvl = 0;

        meleeDamage = meleeLvl * baseMeleeDamage;

        greenKeyObtained = keys[0];
        yellowKeyObtained = keys[1];
        redKeyObtained = keys[2];
        purpleKeyObtained = keys[3];
        whiteKeyObtained = keys[4];

        if (FindObjectOfType<PlayerMovement>())
            FindObjectOfType<PlayerMovement>().UpdateAnimations();
    }

 
    public void UpdateUpgrades(string upgradeName, int id)
    {
        switch (upgradeName)
        {
            case "Health Upgrade": healthUpgrades[id] = true; UpdateStats(); healthPoints++; break;
            case "Ammo Upgrade": ammoUpgrades[id] = true; UpdateStats(); ammo+= 5; break;
            case "Weapon Melee": weaponMeleeUpgrades[id] = true; UpdateStats(); break;
            case "Weapon Range": weaponRangeUpgrades[id] = true; UpdateStats(); ammo = ammoMax; break;
            case "Key": keys[id] = true; UpdateStats(); break;
        }



        //if(upgradeName == "Health Upgrade")
        //{
        //    healthUpgrades[id] = true;
        //    UpdateStats();
        //    healthPoints ++;

        //    Debug.Log(" Tu tienes " + UpgradeCount(healthUpgrades) + " Upgrades de vida ");
        //}

        //if(upgradeName == "Weapon Melee")
        //{
        //    weaponMeleeUpgrades[id] = true;
        //    UpdateStats();

        //    Debug.Log(" Tu tienes " + UpgradeCount(weaponMeleeUpgrades) + " Upgrades de daño ");
        //}

        //if (upgradeName == "Key")
        //{
        //    keys[id] = true;
        //    UpdateStats();
        //}
    }

    public int UpgradeCount(bool[] statusArray)
    {
        int count = 0;
        for(int i = 0; i < statusArray.Length; i++)
        {
            if (statusArray[i])
                count++;
        }
        return count;
    }

    private int GetMaxLvl(bool[] statusArray) //Loop wujuuuu //Repasar Loops\\
    {
        for (int i = statusArray.Length - 1 ; i >= 0; i--)
        {
            if (statusArray[i])
            {
                return i + 1;

            }
        }
        return 0;
    }

    //public int GetUpgrades(string upgradeName) //No funciona ya esta diokis... Borrar
    //{
    //    int upgrades = 0;
    //    if(upgradeName == "Health Upgrade")
    //    {
    //        upgrades = UpgradeCount(healthUpgrades);
    //    }
    //    if (upgradeName == "Weapon Melee")
    //    {
    //        upgrades = UpgradeCount(weaponMeleeUpgrades);
    //    }
        
    //    return upgrades;
    //}

    public void Heal(int points = 1)
    {
        healthPoints = Mathf.Clamp(healthPoints + points, 0, healthPointsMax); //para asegurarnos que no lo cure por encima de hp max
    }
    public void AddAmmo(int amount = 5)
    {
        ammo = Mathf.Clamp(ammo + amount, 0, ammoMax); 
    }

     
}
