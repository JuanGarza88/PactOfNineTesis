using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Testing Options")]
    [SerializeField] [Range(0,10)]float gameSpeed; //Controlar velocidad y lo convert a un slider para mas cool

    public int enterPoint = 0;
    public PlayerMovement.Direction direction = PlayerMovement.Direction.Right;


    private void Awake()
    {
        int objectCount = FindObjectsOfType<GameManager>().Length;
        
        if(objectCount > 1)
        {
            gameObject.SetActive(false); //Lo desactivamos y destruimos.
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); //Cuando pasa de stage, este objeto no puede ser destruido.
        }
    }

    void Start()
    {

    }

    
    void Update()
    {
        Time.timeScale = gameSpeed;
    }

    public void SetNextStage(int nextEnterPoint, PlayerMovement.Direction nextPlayerDirection)
    {
        enterPoint = nextEnterPoint;
        direction = nextPlayerDirection;
    }
}
