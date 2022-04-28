using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{


    public Transform firePoint;
    public GameObject bulletPrefab;

    public float fireRate;
    public float nextFire;

    void Start()
    {
        fireRate = 1f;
        nextFire = Time.time;
    }

    public void Shoot()
    {
        if (Time.time > nextFire)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFire = Time.time + fireRate;
        }
    }
}
