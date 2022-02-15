using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExit : MonoBehaviour
{
    [SerializeField] string nextStageName;
    [SerializeField] PlayerMovement.Direction direction;
    [SerializeField] int enterPoint;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GoToNextStage(nextStageName);
    }

    void GoToNextStage(string stageName)
    {
        FindObjectOfType<GameManager>().SetNextStage(enterPoint, direction);
        SceneManager.LoadScene(stageName); //cadena de texto de los niveles pero que existan en el build
    }

}
