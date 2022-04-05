using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] int baseHealthPoints; //Determinamos cuantos son nuestros puntos de vida base.
    [SerializeField] int baseMeleeDamage;
    public int checkpoint; //Puede ser un string por si necesitamos una referencia especifica

    public bool[] weaponMeleeUpgrades;
    public bool[] healthUpgrades;
    public bool[] keys;

    public bool greenKeyObtained;

    public float healthPointsMax;
    public float healthPoints;

    public int meleeLvl;
    public int meleeDamage;

    //private void Awake()   //Esto ya no\\
    //{
    //    weaponMeleeUpgrades = new bool[] { false, false, false, false }; //Arreflode los upgrdes del melee
    //    healthUpgrades = new bool[] { false, false, false, false, false }; //Arreglo de booleanos 5 items en falso
    //    keys = new bool[] { false, false, false, false, false };
    //}


    public void UpdateStats()
    {
        healthPointsMax = baseHealthPoints + UpgradeCount(healthUpgrades);
        healthPoints = healthPointsMax;

        //Esto es para que haya un orden en las mejoras del arma.
        if (weaponMeleeUpgrades[2])
            meleeLvl = 3;

        else if (weaponMeleeUpgrades[1])
            meleeLvl = 2;

        else if (weaponMeleeUpgrades[0])
            meleeLvl = 1;

        else
            meleeLvl = 0;
 
        meleeDamage = meleeLvl * baseMeleeDamage;

        greenKeyObtained = keys[0];

        if (FindObjectOfType<PlayerMovement>())
            FindObjectOfType<PlayerMovement>().UpdateAnimations();
    }

 
    public void UpdateUpgrades(string upgradeName, int id)
    {
        if(upgradeName == "Health Upgrade")
        {
            healthUpgrades[id] = true;
            UpdateStats();
            healthPoints ++;

            Debug.Log(" Tu tienes " + UpgradeCount(healthUpgrades) + " Upgrades de vida ");
        }
        if(upgradeName == "Weapon Melee")
        {
            weaponMeleeUpgrades[id] = true;
            UpdateStats();

            Debug.Log(" Tu tienes " + UpgradeCount(weaponMeleeUpgrades) + " Upgrades de daño ");
        }
        if (upgradeName == "Key")
        {
            keys[id] = true;
            UpdateStats();
        }
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

    public int GetUpgrades(string upgradeName)
    {
        int upgrades = 0;
        if(upgradeName == "Health Upgrade")
        {
            upgrades = UpgradeCount(healthUpgrades);
        }
        if (upgradeName == "Weapon Melee")
        {
            upgrades = UpgradeCount(weaponMeleeUpgrades);
        }
        return upgrades;
    }

    public void Heal(int points = 1)
    {
        healthPoints = Mathf.Clamp(healthPoints + points, 0, healthPointsMax); //para asegurarnos que no lo cure por encima de hp max
    }
}
