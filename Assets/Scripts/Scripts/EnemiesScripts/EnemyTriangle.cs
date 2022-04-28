using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyTriangle : MonoBehaviour
{
    Killable killable;

    [SerializeField] public Transform target;
    [SerializeField] public float speed = 200f;
    [SerializeField] public float nextWayPointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndPath = false;

    PlayerMovement player;
    Seeker seeker;
    Rigidbody2D rb;
    bool enterOnce = true;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        killable = GetComponent<Killable>();
        player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
        //target = player.transform;

        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (!player && enterOnce)
        {
            player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
            target = player.transform;
            enterOnce = false;
        }
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndPath = true;
            return;
        }
        else
        {
            reachedEndPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        //El enemigo cambia su sprite mirando al jugador
        if (force.x >= 0.01f )
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    } 

}

