using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Killable : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] float hitTime, deadTime, changeTime;
    [SerializeField] Transform sparkPoint; //El efecto de sangre que son "particulas"
    [SerializeField] GameObject sparkPrefab, explosionPrefab; //Guardar el gameobject o sea el prefab
    [SerializeField] Collider2D hitbox;

    [SerializeField] GameObject healthDropPrefab;
    [SerializeField] GameObject ammoDropPrefab;

    Rigidbody2D rb;
    Animator myAnimator;

    Vector2 savedVelocity;

    int animationPhase = 0;
    float damageCounter, changeCounter; 
    bool explosion; //para saber cuando se genera la explosion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        myAnimator.SetFloat("Speed", 1);
        myAnimator.SetLayerWeight(1, 0);
    }


    private void Update()
    {
        DamageAnimation();
        AddExplosions();
    }

    private void DamageAnimation()
    {
        if (damageCounter > 0)
        {
            damageCounter = Mathf.Clamp(damageCounter -= Time.deltaTime, 0, 999);
            changeCounter = Mathf.Clamp(changeCounter -= Time.deltaTime, 0, 999);

            if(changeCounter == 0)
            {
                animationPhase = animationPhase == 0 ? 1 : 0;
                changeCounter = changeTime;
            }
            myAnimator.SetLayerWeight(1, animationPhase);
            if(damageCounter == 0)
            {
                Unfrezze();
                myAnimator.SetFloat("Speed", 1);
                myAnimator.SetLayerWeight(1, 0);
                if (healthPoints == 0)
                   Destroy(gameObject);
            }
        }
            
    }

    public void ProcessDamage(int damage, float direction)
    {
        if (healthPoints == 0)
            return; //return sirve para cortar codigo 

        Debug.Log(damage);
        Frezee();
        healthPoints = Mathf.Clamp(healthPoints - damage, 0, 999);
        AddSpark(direction);
        myAnimator.SetFloat("Speed", 0);
        animationPhase = 1;
        changeCounter = changeTime;

        switch(healthPoints > 0)
        {
            case true: damageCounter = hitTime; break;
            case false: Die(); break;
        }
    }

    void Frezee()
    {
        savedVelocity = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;    //RigidbodyConstraints2D No jala con este pero sin el 2D si jala
    }

    void Unfrezze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; //RigidbodyConstraints2D No jala con este pero sin el 2D si jala
        rb.velocity = savedVelocity;
    }

    void Die()
    {
        hitbox.enabled = false;
        if (GetComponent<Harmful>()) //Buscamos si el enemigo tiene el elemento harmful 
            GetComponent<Harmful>().active = false;
        CreateDrops();
        damageCounter = deadTime;
    }

    private void AddSpark(float direction)
    {
        var newSpark = Instantiate(sparkPrefab, sparkPoint.position, Quaternion.identity);
        newSpark.transform.localScale = new Vector2(direction, 1f);

    }

    private void CreateDrops()
    {
        int randomNumber = Random.Range(1, 11); //1 hasta el 10 el 11 o se incluye
        Debug.Log(randomNumber);
        if(randomNumber < 4)
        {
            Debug.Log("Health Drop");
            var newDrop = Instantiate(healthDropPrefab, sparkPoint.position, Quaternion.identity);
        }
        else if (randomNumber < 7)
        {
            Debug.Log("Ammo Drop");
            var newDrop = Instantiate(ammoDropPrefab, sparkPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No Drop");
        }
    }

    void AddExplosions()
    {
        if (healthPoints > 0)
            return;

        if(damageCounter < .65f && !explosion)
        {
            explosion = true;
            var newExplosion = Instantiate(explosionPrefab, sparkPoint.position, Quaternion.identity);
            newExplosion.transform.localScale = transform.localScale;

        }
    }

    public bool IsStunned()
    {
        return damageCounter > 0;
    }
}
