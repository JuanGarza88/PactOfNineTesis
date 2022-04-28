using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed1;
    [SerializeField] float moveSpeed2; // cuando el jugaddor se acerque va a subir su velocidad
    [SerializeField] float airSpeed; //Movimiento en el aire
    [SerializeField] Vector2 range1, range2; //El primero acelerara el paso y el segundo va hacia el jugador
    [SerializeField] float jumpSpeed, jumpTime; // Velocidad en la que se impulsa y tiempo en la cual salta o sea delay
    
    [Header("Colliders")] //Se necesita saber cuando esta en el piso cuando esta en el aire
    [SerializeField] Collider2D feet;
    [SerializeField] Collider2D groundChecker;
    [SerializeField] Collider2D side;
    [SerializeField] Collider2D edgeChecker;

    Rigidbody2D rb;
    Animator myAnimator;
    Killable killable;

    float animationSpeedMultiplier, jumpCounter;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        killable = GetComponent<Killable>();

        animationSpeedMultiplier = 1;
    }

    void Update()
    {
        if (killable.IsStunned())
            return;

        Move();
        JumpAttack();

        //Debug.Log("Player On Range : " + PlayerOnRange(range1));
        //Debug.Log("Facing player : " + FacingPlayer());
        //Debug.Log("Move speed: " + MoveSpeed());
        //Debug.Log("------------------------");
    }

    private void FixedUpdate()
    {
        if (killable.IsStunned())
            return;

        

        if (!GroundCheckerIsTouchingGround())
            myAnimator.SetBool("Jumping", true);

        if(FeetIsTouchingGround() && rb.velocity.y < 1)
            myAnimator.SetBool("Jumping", false);

        if (IsTouchingWall() || IsCloseToEdge())
            Flip(); //darse la vuelta
    }

    private void Move()
    {
        switch (myAnimator.GetBool("Jumping"))
        {
            case true: rb.velocity = new Vector2(airSpeed * transform.localScale.x, rb.velocity.y); break;
            case false: rb.velocity = new Vector2(MoveSpeed() 
                * transform.localScale.x * animationSpeedMultiplier, rb.velocity.y); jumpCounter = Mathf.Clamp(jumpCounter - Time.deltaTime, 0, 999); break;  /*Calculo que se hace de forma dinamica*/
        }
    }

    private void JumpAttack()
    {
        if (jumpCounter != 0)
            return;

        if(FacingPlayer() && PlayerOnRange(range2) && !myAnimator.GetBool("Jumping"))
        {
            jumpCounter = jumpTime;
            myAnimator.SetBool("Jumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private bool PlayerOnRange(Vector2 range) //Checar que detecte el jugador
    {
        bool onRange = false;
        if (FindObjectOfType<PlayerMovement>())
        {
            var playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
            if (Mathf.Abs(transform.position.x - playerPosition.x) < range.x && Mathf.Abs(transform.position.y - playerPosition.y) < range.y)
                onRange = true;

        }
        return onRange;
            
    }

    private bool FacingPlayer()
    {
         bool facingPlayer = false;
        if (FindObjectOfType<PlayerMovement>()) //Si el enemigo ve a la derecha o si esta viendo a la izquierda
        {
            var playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
            facingPlayer = (transform.position.x - playerPosition.x >= 0 && transform.localScale.x == -1) || (transform.position.x -
                playerPosition.x < 0 && transform.localScale.x == 1);


        }
        return facingPlayer;
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        Move();
    }

    private bool IsTouchingWall()
    {
        return side.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private bool IsCloseToEdge()
    {
        return !myAnimator.GetBool("Jumping") && !edgeChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    private bool FeetIsTouchingGround()
    {
        return feet.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    bool GroundCheckerIsTouchingGround()
    {
        return groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    private float MoveSpeed()
    {
        float speed = moveSpeed1; //La velocidad default
        if (PlayerOnRange(range1) && FacingPlayer())
            speed = moveSpeed2;
        return speed;
    }

    private void SetSpeedMultiplier(float multiplier)
    {
        animationSpeedMultiplier = multiplier;
    }
}
