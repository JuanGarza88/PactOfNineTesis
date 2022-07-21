using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPowerIndicator : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Sprite normalSpite;
    [SerializeField] Sprite FireSpite;
    [SerializeField] Sprite waterSpite;

    [SerializeField] ParticleSystem normalElement;
    [SerializeField] ParticleSystem fireElement;
    [SerializeField] ParticleSystem waterElement;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float showDuration;

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }
    private void OnEnable()
    {
        playerMovement.OnPowerSwitched += ShowPowerIndicator;
    }

    private void OnDisable()
    {
        
        playerMovement.OnPowerSwitched -= ShowPowerIndicator;
    }

    private void ShowPowerIndicator(int power)
    {
       

        if(power == 0)
        {
            spriteRenderer.sprite = normalSpite;
            var NormalParticle = Instantiate(normalElement, transform.position, Quaternion.identity);
            Destroy(NormalParticle.gameObject, 1f);
        }
            
        else if(power == 1)
        {
            spriteRenderer.sprite = FireSpite;
            var FireParticle = Instantiate(fireElement, transform.position, Quaternion.identity);
            Destroy(FireParticle.gameObject, 1f);

        }
        else if(power == 2)
        {
            spriteRenderer.sprite = waterSpite;
            var WaterParticle = Instantiate(waterElement, transform.position, Quaternion.identity);
            Destroy(WaterParticle.gameObject, 1f);

        }
        Debug.Log("New Power" + power);

        spriteRenderer.enabled = true;

        StartCoroutine(HideIndicatorCoroutine());
    }


    private IEnumerator HideIndicatorCoroutine()
    {
        yield return new WaitForSeconds(showDuration);
        spriteRenderer.enabled = false;
    }
}
