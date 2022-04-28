using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] float range = 3;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float numberOfShots = 3;
    

    //Variables para detectar al jugador a una distancia
    [SerializeField] private float distToPlayer;
    [SerializeField] public PlayerMovement player;
    [SerializeField] public Transform target;
    [SerializeField] public bool isAttacking = false;

    EnemyWeapon enemyWeapon;
    bool enterOnce = true;

    void Start()
    {
        enemyWeapon = GetComponent<EnemyWeapon>();
    }

    void Update()
    {
        if (!player && enterOnce)
        {
            enterOnce = false;
            player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
            target = player.transform;
        }
        distToPlayer = Vector2.Distance(transform.position, target.position);

        if (distToPlayer <= range && !isAttacking) //Si el jugador entró al rango del enemigo, y no esta atacando
        {
            isAttacking = true;
            Attack();
        }
    }
    private void Attack()
    {
        if (numberOfShots > 0)
        {
            //numberOfShots = numberOfShots - 1;
            enemyWeapon.Shoot();
            isAttacking = false;
        }
    }

    public float numberOfShotsRemaining()
    {
        return numberOfShots;
    }
    public void updateRemaningShots(float newShot)
    {
        numberOfShots = numberOfShots + newShot;
    }


}