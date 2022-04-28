using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] Vector2 screenSize; //Para config tamaño de pantalla en unidades 15Ancho x 10 altura
    Vector2 stageSize, screenSize;
    private float leftBoundary, rightBoundary, downBoundary, upBoundary; //Limites de la camara

    Transform player;
    GameManager gameManager;
    StageManager stageManager;
    Camera gameCamera;



    public void Initialize()
    {
        gameManager = FindObjectOfType<GameManager>();
        stageManager = GetComponent<StageManager>();
        player = FindObjectOfType<PlayerMovement>().transform; //aquel q contenga en componente del playerMovement
        gameCamera = Camera.main; //para ubicar la camara 

        stageSize = stageManager.size;
        screenSize = gameManager.screenSize;

        SetBoundaries();
    }

    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        //aqui estamos asignando los movimientos de la camara obviamente para que se mueva cn el jugador.
        float newPositionX = Mathf.Clamp(player.position.x, leftBoundary, rightBoundary);
        float newPositionY = Mathf.Clamp(player.position.y + gameManager.cameraOffset, downBoundary, upBoundary);

        gameCamera.transform.position = new Vector3 (newPositionX, newPositionY, gameCamera.transform.position.z);
    }

    void SetBoundaries()
    {
        leftBoundary = screenSize.x / 2;
        rightBoundary = leftBoundary + (screenSize.x * (stageManager.size.x - 1));

        downBoundary = screenSize.y / 2;
        upBoundary = downBoundary + (screenSize.y * (stageManager.size.y - 1));


    }
}
