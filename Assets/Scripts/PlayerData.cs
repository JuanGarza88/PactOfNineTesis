using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool[] weaponMeleeUpgrades;
    public int meleeDamage;

    public bool[] healthUpgrades;

    public float baseHealthPoints;
    public float healtPointsMax;
    
    public float healthPoints;

    private void Awake()
    {
        weaponMeleeUpgrades = new bool[] { false, false, false}; //Arreflode los upgrdes del melee
        healthUpgrades = new bool[] { false, false, false, false, false }; //Arreglo de booleanos 5 items en falso
    }

    void Start()
    {
        meleeDamage = UpgradeCount(weaponMeleeUpgrades) * 2;

        baseHealthPoints = 20;
        healtPointsMax = baseHealthPoints + (UpgradeCount(healthUpgrades) * 5);

        healthPoints = healtPointsMax;
    }

 
    public void UpdateUpgrades(string upgradeName, int id)
    {
        if(upgradeName == "Health Upgrade")
        {
            healthUpgrades[id] = true;
            healtPointsMax = baseHealthPoints + (UpgradeCount(healthUpgrades) * 5);
            healthPoints += 5;

            Debug.Log(" Tu tienes " + UpgradeCount(healthUpgrades) + " Upgrades de vida ");
        }
        if(upgradeName == "Weapon Melee")
        {
            weaponMeleeUpgrades[id] = true;
            meleeDamage = UpgradeCount(weaponMeleeUpgrades) * 2;

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

    public void Heal(int hp)
    {
        healthPoints = Mathf.Clamp(healthPoints + hp, 0, healtPointsMax); //para asegurarnos que no lo cure por encima de hp max
    }
}
