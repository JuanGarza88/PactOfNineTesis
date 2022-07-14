using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle01 : MonoBehaviour
{
    [Header("Main Variables")]
    private CameraController bossCam;
    public Transform bossCamPosition;
    public float bossCamSpeed;
    private PlayerMovement player;
    public Transform boss;
    private bool battleEnded;

    // Phase Transition variables
    [Header("Transitions")]
    public int threshold1;
    public int threshold2;
    public float activeTime, fadeOutTime, inactiveTime; //Revisa cuanto se va tardar en cada estado.
    private float activeCounter, fadeCounter, inactiveCounter;
    public Transform[] spawnPoints; //Arreglo de los puntos Tranform creados.
    private Transform targetPoint; // En la fase 2 y 3, se moverá de spawn points. Este almacena hacia donde va.
    public float moveSpeed;         //Velocidad que se moverá entre targetPoints.

    //Variable del animator.
    [Header("Animator")]
    public Animator anim;
    private bool isFadingOut;
    public bool isDamaged;
    public bool isAttacking;

    //Shots variables
    [Header("Shot variables")]
    public float timeBetweenShots1, timeBetweenShots2; //Para la primera y tercer fase respectivamente.
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;
    private bool resetShotCounter;
    //EnemyWeapon enemyWeapon;


    // Start is called before the first frame update
    void Start()
    {
        bossCam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerMovement>();
        //enemyWeapon = GetComponent<EnemyWeapon>();
        bossCam.BossBattleCameraStatus(true);
        activeCounter = activeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss != null)
        {
            bossCam.BossCamera(bossCam.transform, bossCamPosition, bossCamSpeed);
            BossAnimator();
            BossAnimator();
            if (!battleEnded)
            {
                BossPhaseController();
                Flip();
            }
            else // Comienza el proceso de fin de la batalla.
            {
                EndedBossBattle();
            }
        }
        else
        {
            EndBattle();
        }
    }

    public void EndBattle()
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.EnemyDead3); //por ahora este sonido.
        bossCam.BossBattleCameraStatus(false);
        gameObject.SetActive(false);
    }

    private void BossAnimator()
    {
        if (isFadingOut) // Animación de desaparecer.
        {
            anim.SetTrigger("isVanishing");
            isFadingOut = false;
        }
        if (isDamaged)
        {
            SFXManager.Instance.PlaySFX(SFXManager.SFXName.EnemyHit); 
            anim.SetTrigger("isDamaged");
            isDamaged = false;
        }
        if (isAttacking)
        {
            anim.SetTrigger("isAttacking");
            isAttacking = false;
        }
    }

    private void Flip()
    {
        float observPos;
        observPos = boss.transform.position.x - player.transform.position.x;
        if (observPos < 0)
            boss.transform.localScale = new Vector2(-1f, 1f);
        else
            boss.transform.localScale = new Vector2(1f, 1f);
    }

    private void BossPhaseController()
    {
        //PHASE 1
        if (BossHealthSlider.instance.bossHealth.CurrentHealth() > threshold1) // Si el jefe se encuentra en Fase 1.
        {
            BossHealthSlider.instance.isDamageable = false; // El Boss no podrá ser dañado por default.

            if (activeCounter > 0) // Entra si el Boss se encuentra Activo.
            {
                BossHealthSlider.instance.isDamageable = true;  //Solamente podrá ser dañado cuando el "activeCounter" esta andando. Esto es porque esta en estado no desaparecido.

                activeCounter -= Time.deltaTime;
                if (activeCounter <= 0) // Si ya no esta Activo el Boss. Entrará a fase de FadeOut.
                {
                    fadeCounter = fadeOutTime;
                    isFadingOut = true;
                }

                BossBulletController(); // Revisamos si puede disparar.

                //isAttacking = true;
            }
            else if (fadeCounter > 0) // Entra si el Boss esta en su animacion de FadeOut
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)// Entra si el Boss ya termino su animación de FadeOut. Ahora es invisible.
                {
                    boss.gameObject.SetActive(false);
                    inactiveCounter = inactiveTime;
                    SFXManager.Instance.PlaySFX(SFXManager.SFXName.EnemyDead2); //Sonido de desaparecer.

                }
            }
            else if (inactiveCounter > 0) // Entra si el Boss es invisible.
            {
                inactiveCounter -= Time.deltaTime;
                if (inactiveCounter <= 0) // El contador de invisibilidad terminó. Ahora se hará al Boss visible.
                {
                    boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    boss.gameObject.SetActive(true);
                    SFXManager.Instance.PlaySFX(SFXManager.SFXName.EnemyDead2); //Sonido de desaparecer.

                    activeCounter = activeTime;  //Restablecemos el contador del enemigo activo.
                    resetShotCounter = true; // Activamos el booleano que resetea el contador de Shots.
                }
            }
        }
        // PHASE 2
        else
        {
            if (targetPoint == null)
            {
                targetPoint = boss; //Asignamos su objetivo a donde se moverá a su misma posición.
                fadeCounter = fadeOutTime;
                isFadingOut = true; // Llamamos la animación de desaparecer.
            }
            else
            {

                if (Vector3.Distance(boss.position, targetPoint.position) > 0.2f) // Si Boss esta alejado de la posición objetivo, lo hacemos mover hacia allá.
                {
                    BossHealthSlider.instance.isDamageable = true;
                    if (!isDamaged)
                        boss.position = Vector3.MoveTowards(boss.position, targetPoint.position, moveSpeed * Time.deltaTime); //Movemos al Boss.

                    if (Vector3.Distance(boss.position, targetPoint.position) <= 0.2f)
                    {
                        fadeCounter = fadeOutTime;
                        isFadingOut = true;
                    }

                    BossBulletController();// Revisamos si podemos disparar. PHASE 2 & PHASE 3
                    //enemyWeapon.Shoot();

                    //isAttacking = true;
                }
                else if (fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0)
                    {
                        boss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)
                    {
                        boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Antes de aparecer, el Boss elige un spawnpoint target al azar.
                        while (targetPoint.position == boss.position)
                        {
                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Antes de aparecer, el Boss elige un spawnpoint target al azar.
                        }
                        boss.gameObject.SetActive(true);

                        resetShotCounter = true; // Activamos el booleano que resetea el contador de Shots.
                    }
                }
            }
        }
    }
    private void BossBulletController()
    {
        if (!resetShotCounter)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0) // Si terminó el contador, ya puede disparar un nuevo Shot.
            {
                if (BossHealthSlider.instance.bossHealth.CurrentHealth() > threshold2) // Si esta en la PHASE 1 o PHASE 2, dispara a velocidad normal.
                {
                    shotCounter = timeBetweenShots1;
                }
                else //Si entra a PHASE 3, dispara el doble de rápido.
                {
                    shotCounter = timeBetweenShots2;
                }
                //enemyWeapon.Shoot();
                Instantiate(bullet, shotPoint.position, Quaternion.identity);
            }
        }
        else // Sirve para resetear el contador de Shots. No hay disparo cuando el Boss esta en estado Invisible o en transición.
        {
            shotCounter = timeBetweenShots1;
            resetShotCounter = false;
        }
    }

    public void EndingBossBattle()
    {
        boss.GetComponent<Collider2D>().enabled = false;  //Hacemos que el enemigo ya no colisione con Player o sus disparos.
        battleEnded = true;  // Activamos el final de la batalla.
        fadeCounter = fadeOutTime; //Hacemos ejecutar la animación de FadeOut una ultima vez.
        isFadingOut = true;
    }
    private void EndedBossBattle()
    {
        fadeCounter -= Time.deltaTime;
        if (fadeCounter <= 0)
        {
            EndBattle();
        }
    }

}
