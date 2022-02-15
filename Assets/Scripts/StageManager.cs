using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerPrefab;
    [SerializeField] public Vector2 size;
    [SerializeField] Transform[] enterPonts;

    GameManager gameManager;

    PlayerMovement player;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = Instantiate(playerPrefab, enterPonts[gameManager.enterPoint].position, Quaternion.identity);
        player.direction = gameManager.direction;

        FindObjectOfType<CameraController>().Initialize();

        InitializeStage();
    }

     void InitializeStage()
    {
        var ladders = FindObjectsOfType<Ladder>();
        var platforms = FindObjectsOfType<MovingPlatform>();

        foreach (var ladder in ladders)
        {
            ladder.Initialize();
        }

        foreach (var platform in platforms)
        {
            platform.Initialize();
        }
    }

}
