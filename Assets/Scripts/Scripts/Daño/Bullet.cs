using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{ 
    public float speed = 4f;
    public Rigidbody2D rb;
    bool enterOnce = true;
    [SerializeField] public PlayerMovement player;
    [SerializeField] public EnemyTurret turret;
    [SerializeField] public Transform target;
    [SerializeField] public Vector2 moveDirection;
    void Start()
    {

        //rb.velocity = transform.right  * speed;
    }

    private void Update()
    {
        if (!player && enterOnce)
        {
            enterOnce = false;
            player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
            target = player.transform;
            moveDirection = (target.position - transform.position).normalized; //Se obtiene el la posicion del jugador y se calcula su vector para diparar hacia el
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y)  * speed;
            destroyBullet(3f);
        }
    }

    
    void onTriggerEnter2D(Collider2D hitInfo)
    {
        if(player != null)
        {
            Debug.Log("Hit!");
            player.TakeDamage(transform.position.x);
        }
        destroyBullet(0f);
    }
    

    public void destroyBullet( float bulletDissapear)
    {
        if (bulletDissapear > 0)
        {
            Destroy(gameObject, bulletDissapear);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
