using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] float fireRate;
    [SerializeField] float iniCounter;
    [SerializeField] EnemyProjectile projectilePrefab;
    [SerializeField] Transform projectilePoint;


    Animator myanimator;
    Killable killable;

    float fireCounter;

    private void Start()
    {
        myanimator = GetComponent<Animator>();
        killable = GetComponent<Killable>();

        fireCounter = iniCounter;
    }

    private void Update()
    {
        if (killable.IsStunned())
            return;

        Shoot();
    }

    private void Shoot()
    {
        fireCounter = Mathf.Clamp(fireCounter - Time.deltaTime, 0, 999);
        if(fireCounter == 0)
        {
            fireCounter = fireRate;
            myanimator.Play("Shoot", 0, 0f);
        }
    }

    void CreateProjectile()
    {
        var newProjectile = Instantiate(projectilePrefab, projectilePoint.position, Quaternion.identity);
        newProjectile.transform.localScale = new Vector2(transform.localScale.x, 1f);
        newProjectile.transform.SetParent(FindObjectOfType<Instances>().projectiles);
    }
}
