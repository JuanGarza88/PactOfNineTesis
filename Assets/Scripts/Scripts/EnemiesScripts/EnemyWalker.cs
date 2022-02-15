using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] Collider2D side, edgeChecker;

    Rigidbody2D rb;

    Killable killable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        killable = GetComponent<Killable>();
    }

    void Update()
    {
        //if()

        Walk();
    }

    private void FixedUpdate()
    {
        if (IsTouchingWall() || IsCloseToEdge())
            Flip(); //darse la vuelta
    }

    private void Walk()
    {
        rb.velocity = new Vector2(walkSpeed * transform.localScale.x, rb.velocity.y);
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

    private bool IsCloseToEdge()
    {
        return !edgeChecker.IsTouchingLayers(LayerMask.GetMask("Ground", "Platforms"));
    }

    
}
