using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Killable : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] float hitTime, deadTime, changeTime;
    [SerializeField] Transform sparkPoint; //El efecto de sangre que son "particulas"
    [SerializeField] GameObject sparkPrefab; //Guardar el gameobject 

    [SerializeField] GameObject healthDropPrefab;

    Rigidbody2D rb;
    Animator myAnimator;

    Vector2 savedVelocity;

    int animationPhase = 0;
    float damageCounter, changeCounter; 
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

            }
        }
            
    }

    public void ProcessDamage(int damage, float direction)
    {
        if (healthPoints == 0)
            return; //return sirve para cortar codigo 

        Frezee();
        healthPoints = Mathf.Clamp(healthPoints - damage, 0, 999);
        AddSpark(direction);
        myAnimator.SetFloat("Speed", 0);
        changeCounter = changeTime;

        if (healthPoints > 0)
        {
            damageCounter = hitTime;
        }
        else
        {
            CreateDrops();
            Destroy(gameObject);

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
            var newDrop = Instantiate(healthDropPrefab, sparkPoint.position, Quaternion.identity);

        }


    }

}
