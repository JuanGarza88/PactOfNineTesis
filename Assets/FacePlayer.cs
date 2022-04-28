using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] bool fixedSide;
    [SerializeField] float turnTherehold;

    Killable killable;
    void Start()
    {
        killable = GetComponent<Killable>();
    }

    void Update()
    {
        if (killable.IsStunned())
            return;

        TurnAround();
    }

    private void TurnAround()
    {
        if (fixedSide)
            return;

        if (FindObjectOfType<PlayerMovement>())
        {
            var playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
            if (transform.position.x > playerPosition.x + turnTherehold)
                transform.localScale = new Vector2(-1f, 1f);


            if (transform.position.x < playerPosition.x - turnTherehold)
                transform.localScale = new Vector2(1f, 1f);
        }
    }
}
