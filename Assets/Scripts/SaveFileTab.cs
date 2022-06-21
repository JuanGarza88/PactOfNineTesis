using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveFileTab : MonoBehaviour
{
    [SerializeField] Text newGameText, weaponText;
    [SerializeField] GameObject dataContainer;
    [SerializeField] Image[] heartsImages;
    [SerializeField] Image[] keysImages;

    bool[] weaponMeleeUpgrades;
    bool[] healthUpgrades;
    bool[] keys;

    int healthPointsMax;

    public void ShowSaveData(string saveString)
    {
        //----------------------Incompleto------------------------\\
        bool noData = saveString[0].ToString() == "0";
        if (!noData)
        {
            newGameText.gameObject.SetActive(false);

            string[] saveStrings = saveString.Split('|');
            
            weaponMeleeUpgrades = StringToBoolArray(saveStrings[5]);
            healthUpgrades = StringToBoolArray(saveStrings[3]); //era el 2
            keys = StringToBoolArray(saveStrings[7]); //de 3 le puse 7 y funciono, preguntar mejor
            healthPointsMax = 5 + UpgradeCount(healthUpgrades); //estaba en 4 pero le puse 5

            for (int i = 0; i < heartsImages.Length; i++)
            {
                if (i > healthPointsMax - 1)
                    heartsImages[i].color = Color.clear; //Es el color inverso de white. donde los valores esta en 0,0,0,0
            }
            if (UpgradeCount(weaponMeleeUpgrades) == 0)
                weaponText.text = "--";
            else
                weaponText.text = "x" + UpgradeCount(weaponMeleeUpgrades);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!keys[i])
                    keysImages[i].color = Color.black; //O sea que se mueste que hay un item por desbloquear.
            }
        }
        else
        {
            dataContainer.SetActive(false);
        }
       
    }

    private bool[] StringToBoolArray(string intString)
    {
        bool[] boolArray = new bool[intString.Length];
        for (var i = 0; i < intString.Length; i++)
        {
            boolArray[i] = intString[i].ToString() == "1"; //Si el string es = a 1 lo guardara como verdadero
        }
        return boolArray;
    }

    private int UpgradeCount(bool[] statusArray)
    {
        int count = 0;
        for (int i = 0; i < statusArray.Length; i++)
        {
            if (statusArray[i])
                count++;
        }
        return count;
    }
}
