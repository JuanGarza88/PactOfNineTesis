using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] waypoints;

    PlayerMovement player;
    Collider2D myCollider;

    int nextWaypoint;
    Vector2 previousPosition;
    Vector2 currenPosition;
    public void Initialize()
    {
        player = FindObjectOfType<PlayerMovement>();
        myCollider = GetComponent<Collider2D>();

        nextWaypoint = 0;

        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if(waypoints.Length == 0)
        {
            return;

        }
        Move();
        DragPlayer();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[nextWaypoint].position, moveSpeed * Time.deltaTime);
        if(transform.position == waypoints[nextWaypoint].position)
        {
            nextWaypoint++;
            if(nextWaypoint == waypoints.Length)
            {
                nextWaypoint = 0;
            }
        }
    }

    void DragPlayer()
    {
        currenPosition = transform.position;
        if(myCollider.IsTouching(player.groundChecker) && !player.IsJumping())
        {
            float deltaX = currenPosition.x - previousPosition.x;
            float deltaY = currenPosition.y - previousPosition.y;

            player.Drag(deltaX, deltaY);
        }
        previousPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        if(waypoints.Length > 1)
        {
            for (int waypoint = 0; waypoint < waypoints.Length - 1; waypoint++)
                Gizmos.DrawLine(waypoints[waypoint].position, waypoints[waypoint + 1].position);
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
}
