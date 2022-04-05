using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerPrefab;
    [SerializeField] public Vector2 size;
    [SerializeField] Transform[] enterPonts;
    [SerializeField] MusicPlayer.ThemeName theme;

    GameManager gameManager;

    PlayerMovement player;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = Instantiate(playerPrefab, enterPonts[gameManager.enterPoint].position, Quaternion.identity);
        player.direction = gameManager.direction;
        player.Initialize();

        FindObjectOfType<CameraController>().Initialize();

        MusicPlayer.Instance.PlayTheme(theme);

        InitializeStage();
    }

    public void InitializeStage()
    {
        FindObjectOfType<DataManager>().Initialize();



        var ladders = FindObjectsOfType<Ladder>();
        var platforms = FindObjectsOfType<MovingPlatform>();
        var blocks = FindObjectsOfType<Block>();
        var items = FindObjectsOfType<Item>();
        var checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (var ladder in ladders) //LADDERS//
        {
            ladder.Initialize();
        }

        foreach (var platform in platforms)    //PLATFORMS//
        {
            platform.Initialize();
        }

        foreach (var block in blocks)    //PLATFORMS//
        {
            block.Iniatilize();
        }

        foreach (var item in items)      //ITEMS///
        {
            item.Initialize();
        }

        foreach (var checkpoint in checkpoints)      //CHECKPOINTS//
        {
            checkpoint.Initialize();
        }
    }


}
