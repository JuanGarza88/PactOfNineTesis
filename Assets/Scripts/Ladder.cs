using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Settings")] //Config especificas para cada escalera 
    [SerializeField] public bool hasPlatform; //TerminaEnPlataforma
    [SerializeField] public float upperLimit; //Limite de tolerncia
    [SerializeField] public float thereshold; //no dijo nd aqui 

    [Header("Other")]
    [SerializeField] Color inactive, active;
    [SerializeField] GameObject platformPrefab;


    PlayerMovement player; //Ubicar al player

    Collider2D myCollider;
    SpriteRenderer spriteRenderer;

    public void Initialize()
    {
        player = FindObjectOfType<PlayerMovement>();
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = Color.clear; //El color sea invisible

        if (hasPlatform)
        {
            AddPlatform();
        }
    }

    void Update()
    {
        spriteRenderer.color = inactive;
        if(IsCloseToPlayer() && myCollider.IsTouching(player.groundChecker))
        {
            spriteRenderer.color = active;
            player.SetActiveLadder(gameObject);
        }
        if (!GameManager.Instance.ladderVisible)
            spriteRenderer.color = Color.clear;

    }

    private void AddPlatform()
    {
        GameObject platform = Instantiate(platformPrefab, transform.position,Quaternion.identity); //Quaternion es para que no tenga rotacion
    }

    private bool IsCloseToPlayer()
    {
        return Mathf.Abs(transform.position.x - player.transform.position.x) < thereshold; //Checamos cual es el valor absoluto entre la distancia entre el objeto y el jugador
    }
}
