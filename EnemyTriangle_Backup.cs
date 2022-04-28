using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyTriangle : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] Collider2D side, edgeChecker;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float frequency = 5f;
    [SerializeField] float magnitude = 0.25f;

    Vector3 pos, localScale;
    bool facingRight = true;

    Rigidbody2D rb;

    public AIPath aIPath;
    public bool movingRight;

    Killable killable;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        killable = GetComponent<Killable>();
    }

    void Update()
    {
        //Walk();
        //if (aIPath.desiredVelocity.x >= 0.01f && movingRight)
        if (aIPath.desiredVelocity.x >= 0.01f )
        {
            //movingRight = true;
            //Flip();
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        //else if (aIPath.desiredVelocity.x <= -0.01f && !movingRight == true)
        else if (aIPath.desiredVelocity.x <= -0.01f)
        {
            //movingRight = false;
            //Flip();
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void FixedUpdate()
    {
       // if (IsTouchingWall())
           // Flip(); //darse la vuelta
    }

    private void Walk()
    {
        rb.velocity = new Vector2(walkSpeed * transform.localScale.x, rb.velocity.y);
        rb.position = transform.position + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        Walk();
    }

    private bool IsTouchingWall()
    {
        return side.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    /*private bool IsCloseToEdge()
    {
        return !edgeChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }*/


}

