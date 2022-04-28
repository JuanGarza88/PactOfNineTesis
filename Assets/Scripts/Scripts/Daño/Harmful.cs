using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmful : MonoBehaviour
{
    [HideInInspector] public bool active;

    private void Start()
    {
        active = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!active)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player Hitbox"))
        {
            other.gameObject.GetComponentInParent<PlayerMovement>().TakeDamage(transform.position.x);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!active)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player Hitbox"))
        {
            other.gameObject.GetComponentInParent<PlayerMovement>().TakeDamage(transform.position.x);
        }
    }


}
