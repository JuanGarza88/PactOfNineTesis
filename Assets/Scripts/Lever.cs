using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string id;
    [SerializeField] bool isActive;

    /// LEVER ELEMENT
    /// 
   [SerializeField] int leverElement = 0;


    /// </summary>


    [Header("Other")]
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    //Dos variables de tipo lista
    List<Block> blockList;
    List<Lever> leverList;

    
    PlayerData playerData;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite; //manera reducida de expresar if.

        GetLinkedObjects();
        UpdateLinkedObjects();

        GetComponent<BoxCollider2D>(); // 
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

        

        playerData = FindObjectOfType<PlayerData>();
        if (leverElement == playerData.switchPower)
        {
            Debug.Log("Si son del mismo elemento ");
            SFXManager.Instance.PlaySFX(SFXManager.SFXName.Locking);//CAMBIAR SONIDO DE SWITCHES

            isActive = !isActive;
            spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
            UpdateLinkedObjects();


        }
        else
        {
            Debug.Log("No es");
        }
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
        if (other.GetComponentInParent<PlayerProjectile>())
            other.GetComponentInParent<PlayerProjectile>().Hit();
    }

    //private void DetectElementLever()
    //{
    //    if{leverElement = 0}
    //    {
    //        se
    //    }
    //}

   
}
