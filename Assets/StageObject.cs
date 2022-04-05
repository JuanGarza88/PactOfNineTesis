using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    
    void Start()
    {
        if (!GameManager.Instance.stageObjectsVisible)
            GetComponent<SpriteRenderer>().color = Color.clear;
    }

    
}
