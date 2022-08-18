using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExit : MonoBehaviour
{
    [SerializeField] string nextStageName;
    [SerializeField] PlayerMovement.Direction direction;
    [SerializeField] int enterPoint;

    private bool playerExiting;
    public Transform exitPoint;
    public float movePlayerSpeed;

    public PlayerMovement player;
    private bool playerFound = false;

    private void Start()
    {
        playerExiting = false;
        if(FindObjectOfType<PlayerMovement>() != null)
        {
            player = FindObjectOfType<PlayerMovement>();
            playerFound = true;
        }
    }
    private void Update()
    {
        if (!playerFound)
        {
            player = FindObjectOfType<PlayerMovement>();
            playerFound = true;
        }
        if (playerExiting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       // GoToNextStage(nextStageName);
        if(!playerExiting && !player.IsDead)
        {
            player.canMove = false;
            StartCoroutine(UseDoorCo());
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExiting = true;
        //player.myAnimator.enabled = true;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(1f);
        player.canMove = true;

        UIController.instance.StartFadeFromBlack();

        GoToNextStage(nextStageName);

    }

    void GoToNextStage(string stageName)
    {
        FindObjectOfType<GameManager>().SetNextStage(enterPoint, direction);
        SceneManager.LoadScene(stageName); //cadena de texto de los niveles pero que existan en el build
    }

}
