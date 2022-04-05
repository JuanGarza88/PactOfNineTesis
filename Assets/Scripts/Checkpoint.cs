using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int id; //para diferenciar de otros checkpoints
    [SerializeField] Sprite active, inactive;

    PlayerData playerData;   //se comunica con estos 2.
    DataManager dataManager;

    SpriteRenderer spriteRenderer; //Hace cambios en el sprite renderer

    public void Initialize()
    {

        playerData = FindObjectOfType<PlayerData>(); //se ocupa FindObjectOType pq son objetos que contienen este componente
        dataManager = FindObjectOfType<DataManager>();//que existen en algun lugar de nuestro proyecto.
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = playerData.checkpoint == id ? active : inactive;
    }

    private void ResetCheckpoints()
    {
        var checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (var checkpoint in checkpoints)
        {
            checkpoint.UpdateSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.CheckPoint); //Cambiar sonido de checkpoint.

        if (GameManager.Instance.savingDisabled) //Para que no se guarde.
            return; 

        playerData.Heal(16);
        spriteRenderer.sprite = active;
        playerData.checkpoint = id;
        dataManager.SaveData();
        ResetCheckpoints();
    }
}
