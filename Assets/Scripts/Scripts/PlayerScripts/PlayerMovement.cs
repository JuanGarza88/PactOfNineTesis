using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] string characterName;

    [Header("Colliders")]
    [SerializeField] Collider2D body;
    [SerializeField] Collider2D feet;
    [SerializeField] public Collider2D groundChecker;
    [SerializeField] Collider2D head;
    [SerializeField] Collider2D side;
    [SerializeField] Collider2D damageaArea;


    [Header("Move")]
    [SerializeField] float walkSpeedMax;
    [SerializeField] float acceleration;

    [Header("Climb")]
    [SerializeField] float climbSpeed;
    [SerializeField] float climbingAnimationDuration;


    [Header("DashSpeed")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] int dashAllow; 


    [Header("Jump")]
    [SerializeField] float jumpSpeed;

    [SerializeField] float jumpDownSpeed; //para ponerle en que velocidad cae porque si cae en 0 se vera raro.

    [SerializeField] float jumpImpulse;
    [SerializeField] float jumpEnd;
    [SerializeField] float fallSpeedMax;
    [SerializeField] int jumpsAllowed; //Para saber cuantos saltos tenemos.
    [SerializeField] float gravityScale; //La gravedad que deshabilitaremos y habilitaremos en varios momentos.

    [Header("Damage")]
    [SerializeField] float damageThrowHorizontal; //Cuando el jugador toca un eemento q lo daña hace una reaccion
    [SerializeField] float damageThrowVertical;
    [SerializeField] float recoverTime; //Tiempo de recuperacion, recuperas control
    [SerializeField] float invincibilityTime; //Tiempo que dura el periodo de inbunerabilidad.
    
    private Rigidbody2D rb;
    Animator myAnimator;
    Blinker blinker;

    PlayerData playerData; //en este Script esta la vida del personaje 

    private float recoverCounter;//Que tanto tiempo ha pasado de que se dio ese golpe
    private float invincibilityCounter; //Cuanto tiempo de inbisibilidad queda

    public Direction direction;
    private float walkSpeed; //Esta variable se calcula dinamicamente.

    private float jumpTimeCounter; //Indica cuanto tiempo llevamos presionando el boton.
    private int jumpCounter; //Cuantos saltos hemos dado.
    private bool canGetImpulse;

    private bool isJumpingDown;

    private int dashCounter;

    Ladder activeLadder;
    float climbingTime;

    private string currentAnimation;

    private bool isDashing;
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        blinker = GetComponent<Blinker>();
        playerData = FindObjectOfType<PlayerData>(); //Esta en el GameManager para que no se elimine y se peda pasar de stage.

        UpdateAnimations();
    }

    
    void Update() //Checar lo de el movimiento que la aceleracion comience en 1 y -1///
    {
        currentAnimation = CurrentAnimation();
        currentAnimation = currentAnimation.Remove(0, characterName.Length + 1);

        Recover();
        Invincibility();

        if (currentAnimation != "Attack Melee 1" && currentAnimation != "Jump Attack Melee 1" && currentAnimation != "Attack Range 1" && currentAnimation != "Jump Attack Range 1") //Ponerle el numero solo aplica para current animation
        {
            DamageOff();
            myAnimator.SetBool("Attacking", false);
        }

        if (!myAnimator.GetBool("Hurt"))//Mientas estamos en estado de dolor, no se puede hacer los metodos de abajo.
        {
            Climb();
            if(!myAnimator.GetBool("Climbing"))
            {
                Dash();
                Walk();
                JumpDown();
                Jump();
                ShowJump();
                MeleeAttack();
                RangeAttack();
                FlipPlayer();
            }
        }

        activeLadder = null;
    }

    private void Dash()
    {
        if (isDashing)
            return;

        if (myAnimator.GetBool("Attacking"))
            return;

        if (Input.GetKeyDown(KeyCode.V) && dashCounter < dashAllow) 
        {
            isDashing = true;
            myAnimator.SetBool("Dashing", true);
            float dashVelocity = direction.Equals(Direction.Right) ? dashSpeed : -dashSpeed;
            rb.velocity = new Vector2(dashVelocity, 0f);
            rb.gravityScale = 0f;

            dashCounter++;
            StartCoroutine(DashCorroutine());

            //Reset Jump
            canGetImpulse = false;
            jumpTimeCounter = jumpEnd;
            myAnimator.SetBool("Jumping", false);

        }
               
    }

    private IEnumerator DashCorroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        myAnimator.SetBool("Dashing", false); 
        isDashing = false;

        rb.gravityScale = gravityScale;
    }

    private void FixedUpdate()
    {
        if (isDashing)//
            return;

        if (canGetImpulse)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed + (jumpTimeCounter * jumpImpulse));
            jumpTimeCounter += Time.deltaTime;
            if(jumpTimeCounter > jumpEnd)
            {
                canGetImpulse = false;
                rb.gravityScale = gravityScale;
            }
        }
        else
            rb.gravityScale = gravityScale;

        if (myAnimator.GetBool("Climbing"))
        {
            walkSpeed = 0;
            canGetImpulse = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0; 
        }

       if(!GroundCheckerIsTouchingGround())
       {
            switch (currentAnimation) //Preguntar para que son estas cosas de switch 
            {
                case "Attack Melee 1": myAnimator.Play("Jump Attack Melee", 0, CurrentAnimationTime()); break;
                case "Attack Range 1": myAnimator.Play("Jump Attack Range", 0, CurrentAnimationTime()); break;
            }

            //if (currentAnimation == "Attack Melee 1") es lo mismo que lo de arriba 
            //    //myAnimator.Play("Jump Attack Melee", 0, CurrentAnimationTime());
            //if (currentAnimation == "Attack Range 1")
            //    //myAnimator.Play("Jump Attack Range", 0, CurrentAnimationTime()); 

            if (jumpCounter < 1 && !myAnimator.GetBool("Climbing"))
                jumpCounter = 1;
            myAnimator.SetBool("Jumping", true);
       }

       if(FeetIsTouchingGround())
       {
            switch (currentAnimation) //Preguntar para que son estas cosas de switch 
            {
                case "Jump Attack Melee 1": myAnimator.Play("Attack Melee", 0, CurrentAnimationTime()); break;
                case "Jump Attack Range 1": myAnimator.Play("Attack Range", 0, CurrentAnimationTime()); break;
            }

            //if (currentAnimation == "Jump Attack Melee 1") es lo mismo
            //    myAnimator.Play("Attack Melee", 0, CurrentAnimationTime());
            //if (currentAnimation == "Jump Attack Range 1")
            //    myAnimator.Play("Attack Range", 0, CurrentAnimationTime());

            jumpCounter = 0;
            myAnimator.SetBool("Jumping", false);

            dashCounter = 0;

       }

        if (IsTouchingCeiling())
        {
            canGetImpulse = false;
        }

    }

    private void Recover()
    {
        recoverCounter = Mathf.Clamp(recoverCounter - Time.deltaTime, 0, 999);
        if (recoverCounter == 0)
            myAnimator.SetBool("Hurt", false);        
    }

    private void Invincibility()
    {
        invincibilityCounter = Mathf.Clamp(invincibilityCounter - Time.deltaTime, 0, 999);
        if(invincibilityCounter == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            blinker.enabled = false;
        }
    }

    private void Climb()
    {
        
        if(ClimbingLadder() && !myAnimator.GetBool("Attacking"))
        {
            jumpCounter = 0;
            myAnimator.SetBool("Climbing", true);
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Jumping", false);

            
        }
        else if(!activeLadder || Input.GetButtonDown("Jump"))
        {
            myAnimator.SetBool("Climbing", false);
        }
        if (myAnimator.GetBool("Climbing"))
        {
            float deltaY; //Distancia que se movera en Y.
            float newPosY; //Donde estara la posicion para que no se salga de ciertos parametros.
            float upperLimit = activeLadder.transform.position.y - activeLadder.upperLimit;
            float bottomLimit = activeLadder.transform.position.y - activeLadder.transform.localScale.y;

            deltaY = Input.GetAxis("Vertical") * climbSpeed * Time.deltaTime;
            if (IsTouchingCeiling() && Input.GetAxis("Vertical") > 0)
                deltaY = 0;
            if (transform.position.y > upperLimit)
                transform.position = new Vector2(transform.position.x, upperLimit);
            newPosY = transform.position.y + deltaY;

            if (activeLadder.hasPlatform)
            {
                if(newPosY > upperLimit)
                {
                    newPosY = activeLadder.transform.position.y;
                    myAnimator.SetBool("Climbing", false );
                }
            }

            else if (newPosY > upperLimit)
            {
                deltaY = 0;
                newPosY = Mathf.Clamp(newPosY, bottomLimit, upperLimit);
            }
            if(newPosY <= bottomLimit)
            {
                newPosY = bottomLimit;
                myAnimator.SetBool("Climbing", false);
            }
            transform.position = new Vector2(activeLadder.transform.position.x, newPosY);

            if (deltaY != 0)
                ClimbAnimation();

            myAnimator.Play("Climb", 0, (1 / climbingAnimationDuration) * climbingTime);
        }
    }

    private void ClimbAnimation()
    {
        climbingTime += Input.GetAxis("Vertical") * Time.deltaTime;
        if (climbingTime > climbingAnimationDuration)
            climbingTime -= climbingAnimationDuration;

        else if (climbingTime < 0)
            climbingTime += climbingAnimationDuration;
    }

    private bool ClimbingLadder()
    {
        bool climbingLadder = false;
        if (activeLadder && !myAnimator.GetBool("Hurt"))
        {
            if (Input.GetAxis("Vertical") > 0 && activeLadder.transform.position.y > transform.position.y + .05f)
            {
                climbingLadder = true;
            }
            else if (Input.GetAxis("Vertical") < 0 && (!GroundCheckerIsTouchingGround() || Mathf.Abs(activeLadder.transform.position.y - transform.position.y) < .05f))
            {
                climbingLadder = true;
            }
        }

        return climbingLadder;
    }

    public void SetActiveLadder(GameObject ladder)
    {
        activeLadder = ladder.GetComponent<Ladder>();
    }

    private void Walk()
    {
        if (isDashing)
            return;
        //if (Input.GetAxis("Horizontal") < 0 && currentAnimation != "Attack Melee 1")
        if ((Input.GetAxis("Horizontal") < 0 && currentAnimation != "Attack Melee 1") && ((Input.GetAxis("Horizontal") < 0 && currentAnimation != "Attack Range 1"))) //Detenemos el walk-cycle a la izquierda cuando presiona disparo
            {
                direction = Direction.Left;
            walkSpeed -= acceleration * Time.deltaTime;
            if (walkSpeed > 0)
                walkSpeed -= acceleration * Time.deltaTime;
        }
        //else if (Input.GetAxis("Horizontal") > 0 && currentAnimation != "Attack Melee 1")
        else if ((Input.GetAxis("Horizontal") > 0 && currentAnimation != "Attack Melee 1") && ((Input.GetAxis("Horizontal") > 0 && currentAnimation != "Attack Range 1"))) //Detenemos el walk-cycle a la izquierda cuando presiona disparo
        {
            direction = Direction.Right;
            walkSpeed += acceleration * Time.deltaTime;
            if (walkSpeed < 0)
                walkSpeed += acceleration * Time.deltaTime;
        }
        else if (walkSpeed != 0)
        {
            //Sing nos regresa 1 positivo o 1 negativo//
            walkSpeed -= (acceleration * Time.deltaTime * .75f) * Mathf.Sign(walkSpeed);
            if (Mathf.Abs(walkSpeed) < 1)
                walkSpeed = 0;
        }
        if (IsTouchingWall())
            walkSpeed = 0;

        walkSpeed = Mathf.Clamp(walkSpeed, -walkSpeedMax, walkSpeedMax);
        myAnimator.SetBool("Walking",Input.GetAxis("Horizontal") !=0 && !IsTouchingWall());
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        
    }

    void Jump()
    {
        if (isDashing)
            return;

        if (isJumpingDown)
            return;

        if (Input.GetButtonDown("Jump") && jumpCounter < jumpsAllowed) //El contador sea menor a los saltos permitidos.
        {
            jumpCounter++; //es lo mismo poner ++ y = jumpCounter + 1;
            canGetImpulse = true;
            rb.gravityScale = 0;
            jumpTimeCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            myAnimator.SetBool("Jumping", true);
        }
        if (Input.GetButtonUp("Jump"))
            canGetImpulse = false;
  
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, fallSpeedMax, 999f));
    }

    private void JumpDown()
    {
        isJumpingDown = false;
        if(groundChecker.IsTouchingLayers(LayerMask.GetMask("Platforms")) && !groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if(Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0)
            {
                isJumpingDown = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpDownSpeed);
                transform.position = new Vector2(transform.position.x, transform.position.y - .4f);
                myAnimator.SetBool("Jumping", true);
            }
        }
    }

    private void ShowJump()
    {
        if (myAnimator.GetBool("Jumping") && !myAnimator.GetBool("Attacking"))
        {
            if (Mathf.Abs(rb.velocity.y) > 8 || canGetImpulse)
                myAnimator.Play("Jump", 0, 0f);
            else
                myAnimator.Play("Jump" , 0, 0.5f);
        }
    }

    private void MeleeAttack()
    {
        if (isDashing)
            return;

        if (playerData.GetUpgrades("Weapon Melee") < 1) // Si no tienes el arma no puedes pegar o sea si no tienes el upgrade
            return;

        if (Input.GetButtonDown("Attack Melee") && !myAnimator.GetBool("Attacking"))
        {
            myAnimator.SetBool("Attacking", true);
            if (myAnimator.GetBool("Jumping"))
                myAnimator.Play("Jump Attack Melee", 0, 0f); 
            else
            myAnimator.Play("Attack Melee", 0, 0f); 
        }
    }

    private void RangeAttack()
    {
       if (isDashing)
            return;

        //if (playerData.GetUpgrades("Weapon Melee") < 1) // Si no tienes el arma no puedes pegar o sea si no tienes el upgrade
        //    return;

        if (Input.GetButtonDown("Attack Range") && !myAnimator.GetBool("Attacking"))
        {
            myAnimator.SetBool("Attacking", true);
            if (myAnimator.GetBool("Jumping"))
                myAnimator.Play("Jump Attack Range", 0, 0f);
            else
                myAnimator.Play("Attack Range", 0, 0f);
        }
    }

    void FlipPlayer()
    {
        if (direction == Direction.Left && !myAnimator.GetBool("Attacking"))
            transform.localScale = new Vector2(-1, 1);

        if (direction == Direction.Right && !myAnimator.GetBool("Attacking"))
            transform.localScale = new Vector2(1, 1);
    }

    public void Drag(float deltaX, float deltaY)
    {
        float newPositionX = transform.position.x + deltaX;
        float newPositionY = transform.position.y + deltaY;

        transform.position = new Vector2(newPositionX, newPositionY);
    }

    private bool GroundCheckerIsTouchingGround()
    {
        return groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms")); //Le puse plataforms jajajxD es Platforms pñts
    }
    private bool FeetIsTouchingGround()
    {
        return feet.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    private bool IsTouchingWall()
    {
        return side.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private bool IsTouchingCeiling()
    {
        return head.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public bool IsJumping()
    {
        return myAnimator.GetBool("Jumping");
    }

    private void DamageOn()
    {
        damageaArea.enabled = true;
    }

    private void DamageOff()
    {
        damageaArea.enabled = false;
    }
    
    public void TakeDamage(float positionX) //Cuando te pegan.
    {
        if (InvincibilityOn() || GameManager.Instance.invincibility)
            return;

        direction = positionX > transform.position.x ? Direction.Right : Direction.Left;
        FlipPlayer();

        playerData.healthPoints = Mathf.Clamp(playerData.healthPoints - 1, 0, 999);


        walkSpeed = 0;
        jumpCounter++;
        canGetImpulse = false;
        rb.velocity = new Vector2(damageThrowHorizontal * transform.localScale.x * -1f, damageThrowVertical);
        myAnimator.SetBool("Jumping", true);
        myAnimator.SetBool("Walking", false);
        myAnimator.SetBool("Hurt", true);
        recoverCounter = recoverTime;
        invincibilityCounter = invincibilityTime;
        blinker.enabled = true;

        SFXManager.Instance.PlaySFX(SFXManager.SFXName.PlayerHurt); //Cambiar audio o sea el sonido.

        if (playerData.healthPoints <= 0)
        {
            FindObjectOfType<DataManager>().LoadData();
        }
    }

    private bool InvincibilityOn()
    {
        return invincibilityCounter > 0;
    }

    string CurrentAnimation() //Cual es el nombre de a animacion que se esta reproduciendo.
    {
        string animationName = ""; //Cadena vacia
        AnimatorClipInfo[] currentClipInfo = myAnimator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo.Length > 0)
            animationName = currentClipInfo[0].clip.name;
        return animationName;
    }

    public float CurrentAnimationTime()
    {
        return myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Target")) //donde golpeamos
        {
            Debug.Log(transform.lossyScale.x * -1);
            if (other.GetComponentInParent<Killable>())
                other.GetComponentInParent<Killable>().ProcessDamage(playerData.meleeDamage, Mathf.Sign(transform.lossyScale.x * -1));

        }
    }

    public void UpdateAnimations()
    {
        myAnimator.SetLayerWeight(1, playerData.meleeLvl == 2 ? 1 : 0);
        myAnimator.SetLayerWeight(2, playerData.meleeLvl == 3 ? 1 : 0);
    }

    public enum Direction //para guardar en una variable uno de los dos valores
    {
        Right,
        Left
    }
}
