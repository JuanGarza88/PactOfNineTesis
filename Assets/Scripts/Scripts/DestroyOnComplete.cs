using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnComplete : MonoBehaviour
{
    private void OnComplete()
    {
        Destroy(gameObject); //este metodo es llamado en a animacion de la paricula del efecto como un evento. 
    }
}
