using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle01 : MonoBehaviour
{
    private CameraController bossCam;
    public Transform bossCamPosition;
    public float bossCamSpeed;
    private PlayerMovement player;

    // Phase Transition variables
    public int threshold1, threshold2;
    public float activeTime, fadeOutTime, inactiveTime; //Revisa cuanto se va tardar en cada estado.
    private float activeCounter, fadeCounter, inactiveCounter;
    public Transform[] spawnPoints; //Arreglo de los puntos Tranform creados.
    private Transform targetPoint; // En la fase 2 y 3, se moverá de spawn points. Este almacena hacia donde va.
    public float moveSpeed;         //Velocidad que se moverá entre targetPoints.

    //Variable del animator.
    public Animator anim;
    private bool isFadingOut;
    public bool isDamaged;

    public Transform boss;
    private bool battleEnded;


    // Start is called before the first frame update
    void Start()
    {
        bossCam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerMovement>();
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
            anim.SetTrigger("isDamaged");
            isDamaged = false;
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
        if (BossHealthSlider.instance.killObject.CurrentHealth() > threshold1) // Si el jefe se encuentra en Fase 1.
        {
            //BossHealthController.instance.isDamageable = false; // El Boss no podrá ser dañado por default.

            if (activeCounter > 0) // Entra si el Boss se encuentra Activo.
            {
                //BossHealthController.instance.isDamageable = true;  //Solamente podrá ser dañado cuando el "activeCounter" esta andando. Esto es porque esta en estado no desaparecido.

                activeCounter -= Time.deltaTime;
                if (activeCounter <= 0) // Si ya no esta Activo el Boss. Entrará a fase de FadeOut.
                {
                    fadeCounter = fadeOutTime;
                    isFadingOut = true;
                }

                //BossBulletController(); // Revisamos si puede disparar.
            }
            else if (fadeCounter > 0) // Entra si el Bos esta en su animacion de FadeOut
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)// Entra si el Boss ya termino su animación de FadeOut. Ahora es invisible.
                {
                    boss.gameObject.SetActive(false);
                    inactiveCounter = inactiveTime;
                }
            }
            else if (inactiveCounter > 0) // Entra si el Boss es invisible.
            {
                inactiveCounter -= Time.deltaTime;
                if (inactiveCounter <= 0) // El contador de invisibilidad terminó. Ahora se hará al Boss visible.
                {
                    boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    boss.gameObject.SetActive(true);

                    activeCounter = activeTime;  //Restablecemos el contador del enemigo activo.
                    //resetShotCounter = true; // Activamos el booleano que resetea el contador de Shots.
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
                //player.isDamageable = false;

                if (Vector3.Distance(boss.position, targetPoint.position) > 0.2f) // Si Boss esta alejado de la posición objetivo, lo hacemos mover hacia allá.
                {
                    //BossHealthController.instance.isDamageable = true;
                    if (!isDamaged)
                        boss.position = Vector3.MoveTowards(boss.position, targetPoint.position, moveSpeed * Time.deltaTime); //Movemos al Boss.

                    if (Vector3.Distance(boss.position, targetPoint.position) <= 0.2f)
                    {
                        fadeCounter = fadeOutTime;
                        isFadingOut = true;
                    }

                    //BossBulletController();// Revisamos si podemos disparar. PHASE 2 & PHASE 3
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

                        //resetShotCounter = true; // Activamos el booleano que resetea el contador de Shots.
                    }
                }
            }
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
