using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float therehold;

    [Header("Damage")]
    [SerializeField] int damage;


    [Header("Explosion")]
    [SerializeField] GameObject explosionPrefab;

    StageManager stageManager;
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    void Update()
    {
        transform.position += transform.right * (moveSpeed * Time.deltaTime * transform.localScale.x);
        if (IsOutOfStageBounds())
            Destroy(gameObject);
    }

    private bool IsOutOfStageBounds()
    {
        return transform.localScale.x == -1 && transform.position.x < stageManager.leftBoundary - therehold ||
            transform.localScale.x == 1 && transform.position.x > stageManager.rightBoundary + therehold;

    }

    void AddSpark()
    {
        var newSpark = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newSpark.transform.SetParent(FindObjectOfType<Instances>().effects);
    }

    void HitTarget(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Target")) //donde golpeamos
        { 
           // Debug.Log(transform.lossyScale.x * -1);
            if (other.GetComponentInParent<IDamagable>() != null)
            {
                other.GetComponentInParent<IDamagable>().ProcessDamage(damage, Mathf.Sign(transform.lossyScale.x * -1));
                Hit();
            }

            //else if (other.GetComponent<Lever>()) //Borrar
            //{
            //    //other.GetComponent<Lever>().TriggerLever(); //Borrar
            //    AddSpark();
            //    Destroy(gameObject);
            //}
        }
    }

    void HitGround(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Hit();
        }
    }
    public void Hit()
    {
        AddSpark();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitTarget(other);
        HitGround(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HitTarget(other);
        HitGround(other);
    }
}
