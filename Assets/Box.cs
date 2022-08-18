using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] int startHealth;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = startHealth;
    }
    public int CurrentHealth()
    {
        return currentHealth;
    }

    public bool IsStunned()
    {
        return false;
    }

    public void ProcessDamage(int damage, float direction)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
