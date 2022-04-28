using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float therehold;

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

    void HitPlayer(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Hitbox"))
            other.GetComponentInParent<PlayerMovement>().TakeDamage(transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitPlayer(other);
        AddSpark();
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HitPlayer(other);
        AddSpark();
        Destroy(gameObject);
    }

    
}
