using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    void ProcessDamage(int damage, float direction);
    bool IsStunned();
    int CurrentHealth();
}
