using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] int baseHealthPoints; //Determinamos cuantos son nuestros puntos de vida base.

    public int checkpoint; //Puede ser un string por si necesitamos una referencia especifica

    public bool[] weaponMeleeUpgrades;
    public bool[] healthUpgrades;
    public bool[] keys;

    public bool greenKeyObtained;

    public float healtPointsMax;
    public float healthPoints;

    
    public int meleeDamage;

    //private void Awake()   //Esto ya no\\
    //{
    //    weaponMeleeUpgrades = new bool[] { false, false, false, false }; //Arreflode los upgrdes del melee
    //    healthUpgrades = new bool[] { false, false, false, false, false }; //Arreglo de booleanos 5 items en falso
    //    keys = new bool[] { false, false, false, false, false };
    //}


    public void UpdateStats()
    {
        Debug.Log(weaponMeleeUpgrades[0]);
        Debug.Log(weaponMeleeUpgrades[1]);
        Debug.Log(weaponMeleeUpgrades[2]);

        healtPointsMax = baseHealthPoints + UpgradeCount(healthUpgrades);
        healthPoints = healtPointsMax;
 
        meleeDamage = UpgradeCount(weaponMeleeUpgrades) * 2;

        greenKeyObtained = keys[0];
    }

 
    public void UpdateUpgrades(string upgradeName, int id)
    {
        if(upgradeName == "Health Upgrade")
        {
            healthUpgrades[id] = true;
            healtPointsMax = baseHealthPoints + UpgradeCount(healthUpgrades);
            healthPoints ++;

            Debug.Log(" Tu tienes " + UpgradeCount(healthUpgrades) + " Upgrades de vida ");
        }
        if(upgradeName == "Weapon Melee")
        {
            weaponMeleeUpgrades[id] = true;
            meleeDamage = UpgradeCount(weaponMeleeUpgrades) * 2;
        }
        if (upgradeName == "Key")
        {
            keys[id] = true;
            if (id == 0)
                greenKeyObtained = true;
        }
    }

    int UpgradeCount(bool[] statusArray)
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

    public void Heal()
    {
        healthPoints = Mathf.Clamp(healthPoints + 1, 0, healtPointsMax); //para asegurarnos que no lo cure por encima de hp max
    }
}
