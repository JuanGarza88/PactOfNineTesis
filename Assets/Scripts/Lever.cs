using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string id;
    [SerializeField] bool isActive;


    [Header("Other")]
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    //Dos variables de tipo lista
    List<Block> blockList;
    List<Lever> leverList;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;

        GetLinkedObjects();
        UpdateLinkedObjects();
    }

    public void SetActive(bool active)
    {
        isActive = active;
        GetComponent<SpriteRenderer>().sprite = isActive ? activeSprite : inactiveSprite; 
    }

    private void GetLinkedObjects()
    {
        blockList = new List<Block> { };
        leverList = new List<Lever> { };

        Block[] stageBlocks = FindObjectsOfType<Block>();
        foreach(var block in stageBlocks)
        {
            if (block.id == id)
                blockList.Add(block);
        }

        Lever[] stageLevers = FindObjectsOfType<Lever>();
        foreach (var lever in stageLevers)
        {
            if (lever.id == id)
                leverList.Add(lever);
        }
    }

    public void TriggerLever()
    {
        //if (isActive)  //esto es por si  lo queremos activar 1 vez y se quede para siempre activado
        //    return; 

        SFXManager.Instance.PlaySFX(SFXManager.SFXName.Locking);//CAMBIAR SONIDO DE SWITCHES
        isActive = !isActive;
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
        UpdateLinkedObjects();
    }

    private void UpdateLinkedObjects()
    {
        //Por cada bloque dentro de mi lista de bloques
        foreach (var block in blockList) 
            block.SetActive(isActive);

        foreach (var lever in leverList)
            lever.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerLever();
    }
}
