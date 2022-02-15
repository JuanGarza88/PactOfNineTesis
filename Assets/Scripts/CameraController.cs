using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 screenSize; //Para config tamaño de pantalla en unidades 15Ancho x 10 altura

    [SerializeField] float cameraOffset; 

    [SerializeField] float leftBoundary, rightBoundary, downBoundary, upBoundary; //Limites de la camara

    Transform player;
    StageManager stageManager;
    Camera gameCamera;



    public void Initialize()
    {
        stageManager = GetComponent<StageManager>();
        player = FindObjectOfType<PlayerMovement>().transform; //aquel q contenga en componente del playerMovement
        gameCamera = Camera.main; //para ubicar la camara 

        SetBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        //aqui estamos asignando los movimientos de la camara obviamente para que se mueva cn el jugador.
        float newPositionX = Mathf.Clamp(player.position.x, leftBoundary, rightBoundary);
        float newPositionY = Mathf.Clamp(player.position.y + cameraOffset, downBoundary, upBoundary);

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
