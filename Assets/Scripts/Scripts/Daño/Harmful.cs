using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmful : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Hitbox"))
        {
            other.gameObject.GetComponentInParent<PlayerMovement>().TakeDamage(transform.position.x);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Hitbox"))
        {
            other.gameObject.GetComponentInParent<PlayerMovement>().TakeDamage(transform.position.x);
        }
    }


}
