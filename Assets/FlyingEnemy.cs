using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed, range;
    [SerializeField] Collider2D side;
    [SerializeField] Transform leftPoint, rightPoint;

    Rigidbody2D rb;

    Killable killable;

    private float iniPositionX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        killable = GetComponent<Killable>();

        SetRoute();
    }


    void Update()
    {
        if (killable.IsStunned())
            return;

        Fly();
    }

    private void FixedUpdate()
    {
        if (killable.IsStunned())
            return;

        if (IsTouchingWall())
            Flip();
        else if (IsOutOfRange())
        {
            transform.position = new Vector2(iniPositionX + (range * transform.localScale.x), transform.position.y);
            Flip();
        }
    }

    private void Fly()
    {
        rb.velocity = new Vector2(moveSpeed * transform.localScale.x, rb.velocity.y);
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        Fly();
    }

    private bool IsTouchingWall()
    {
        return side.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private bool IsOutOfRange()
    {
        return Mathf.Abs(iniPositionX - transform.position.x) > range && range != 0;
    }

    private void SetRoute()
    {
        if (range == 0)
            return;

        iniPositionX = transform.position.x;
        leftPoint.position = new Vector2(iniPositionX - range, transform.position.y);
        rightPoint.position = new Vector2(iniPositionX + range, transform.position.y);

        //para guardar los waypoints del enemigo
        leftPoint.transform.SetParent(FindObjectOfType<Instances>().waypoints);
        rightPoint.transform.SetParent(FindObjectOfType<Instances>().waypoints);
    }
}
