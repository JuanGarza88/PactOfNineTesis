using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite : MonoBehaviour
{
    [SerializeField] float walkSpeed, range = 3, timeBetweenAttacks;
    [SerializeField] Collider2D side, edgeChecker;

    //Variables para detectar al jugador a una distancia
    [SerializeField] private float distToPlayer;
    [SerializeField] public PlayerMovement player;
    [SerializeField] public Transform target;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isJumping = false;
    [SerializeField] private float jumpHeight;

    Rigidbody2D rb;
    EnemyWeapon enemyWeapon;
    bool enterOnce = true;

    Killable killable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        killable = GetComponent<Killable>();
        enemyWeapon = GetComponent<EnemyWeapon>();

        jumpHeight = 7f;
    }

    void Update()
    {
        if (!player && enterOnce)
        {
            enterOnce = false;
            player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
            target = player.transform;
        }
        Walk();
        distToPlayer = Vector2.Distance(transform.position, target.position);

        if (distToPlayer <= range && !isAttacking) //Si el jugador entró al rango del enemigo, y no esta atacando
        {
            isAttacking = true;
            if (target.position.x > transform.position.x && transform.localScale.x > 0 ||
                target.position.x < transform.position.x && transform.localScale.x < 0) //Si el enemigo no está mirando hacia el jugador, lo voltea a ver.
            {
                Flip();
            }
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            if (IsTouchingWall() || IsCloseToEdge())
                Flip(); //darse la vuelta
        }
        if (!edgeChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms")))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    private void Walk()
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(-walkSpeed * transform.localScale.x, rb.velocity.y);
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        //transform.Rotate(0f, 180f, 0f);

    }

    private bool IsTouchingWall()
    {
        return side.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    private bool IsCloseToEdge()
    {
        return !edgeChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    IEnumerator Attack()
    {
        
        if (!isJumping)
        {
            enemyWeapon.Shoot();
            timeBetweenAttacks = Random.Range(3, 4);//Numero de tiempo aleatorio para esperar a que ataque el enemigo.
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            yield return new WaitForSeconds(timeBetweenAttacks);
            isAttacking = false;
        }
    }
}
