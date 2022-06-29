using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Testing Options")]
    [SerializeField] [Range(0,10)]float gameSpeed; //Controlar velocidad y lo convert a un slider para mas cool.
    [SerializeField] public bool useTestValues;
    [SerializeField] public bool stageObjectsVisible;
    [SerializeField] public bool ladderVisible; //Los bjetos que muestran el estado de la escalera.
    [SerializeField] public bool savingDisabled;
    [SerializeField] public bool invincibility;
    [SerializeField] public bool infiniteHealth;

    [Header("World")]
    [SerializeField] public Vector2 screenSize;
    [SerializeField] public float cameraOffset;

    public int enterPoint = 0;
    public PlayerMovement.Direction direction = PlayerMovement.Direction.Right;

    public int saveSlot;

    static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

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
            instance = this;
            DontDestroyOnLoad(gameObject); //Cuando pasa de stage, este objeto no puede ser destruido.
            saveSlot = 1;
        }
    }
    
    void Update()
    {
        TimeController();
    }

    // Funcion que controla la velocidad del juego.
    // Se agregó para detectar si el usuario activó la pausa:
    //      Si esta en Pausa, velocidad del juego = 0f.
    //      Si se quitó la pausa, se devuelve el valor default.
    private void TimeController()
    {
        if (UIController.instance != null)
        {
            if (UIController.instance.isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = gameSpeed;
            }
        }
        else
        {
            Time.timeScale = gameSpeed;
        }
    }

    public void SetNextStage(int nextEnterPoint, PlayerMovement.Direction nextPlayerDirection)
    {
        enterPoint = nextEnterPoint;
        direction = nextPlayerDirection;
    }

    public string SaveSlotKey()
    {
        return "Player Data " + saveSlot;
    }
}
