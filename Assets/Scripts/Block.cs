using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //[SerializeField] public string id;

    [Header("Settings")]
    [SerializeField] public string id;
    [SerializeField] bool isActive;


    [Header("Other")]
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    PlayerData playerData;

    public void Iniatilize() //Iniatilize
    {
        playerData = FindObjectOfType<PlayerData>();
        if(id == "Green")
        {
            if (playerData.greenKeyObtained)
            {
                SetActive(false);
            }
        }
    }

    public void Unlock()
    {
        if (!isActive)
            return;

        SetActive(false);
    }

    public void SetActive(bool active)
    {
        isActive = active;
        GetComponent<SpriteRenderer>().sprite = isActive ? activeSprite : inactiveSprite;
        GetComponent<Collider2D>().enabled = active;
    }
}
