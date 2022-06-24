using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyedollBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public int damageAmount;
    public GameObject impactFX;
    private PlayerMovement player;
    bool enterOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBossBulletVariables();

        //AudioManager.instance.PlaySFXAdjusted(2); // "Boss Shot" SFX
    }

    // Update is called once per frame
    void Update()
    {
        BossBulletMovement();

    }

    private void LateUpdate()
    {
        if (!player && enterOnce)
        {
            enterOnce = false;
            player = FindObjectOfType<PlayerMovement>(); //Obtenemos la instancia del jugador para que el enemigo siga su tranform
        }
    }

    private void BossBulletMovement()
    {
        rb.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //AudioManager.instance.PlaySFXAdjusted(3); // "Bullet Impact" SFX

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Se daño al Player");
            //PlayerHealthController.instance.DamagedPlayer(damageAmount);
        }
        BossBulletCollided();

    }

    public void BossBulletCollided()
    {
        if (impactFX != null)
        {
            impactFX.transform.localScale = new Vector2(1f, 1f);
            Instantiate(impactFX, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    private void InitializeBossBulletVariables()
    {
        transform.localScale = new Vector2(1f, 1f);
        rb.transform.localScale = new Vector2(1f, 1f);
        if (!player && enterOnce)
        {
            enterOnce = false;
            player = FindObjectOfType<PlayerMovement>();
            //Similar al EnemyFlyer, el BossBullet siempre mirará hacia la dirección del jugador.
            Vector3 direction = transform.position - player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


    }
}
