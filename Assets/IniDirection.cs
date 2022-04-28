using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniDirection : MonoBehaviour
{
    //Todos los enemigos que tengan este componente voltearan a ver al jugador cuando se cargue la partida.
    [SerializeField] Direction direction;

    public void Initialize()
    {
        switch (direction)
        {
            case Direction.None: FacePlayer(); break;
            case Direction.Left: transform.localScale = new Vector2(-1f, 1f); break;
            case Direction.Right: transform.localScale = new Vector2(1f, 1f); break;
        }
    }

    void FacePlayer()
    {
        var playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
        if (transform.position.x > playerPosition.x)
            transform.localScale = new Vector2(-1f, 1f);

        if (transform.position.x < playerPosition.x)
            transform.localScale = new Vector2(1f, 1f);

    }
    enum Direction
    {
        None,
        Left,
        Right
    }
}
