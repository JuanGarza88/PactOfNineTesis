using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip ammo;
    [SerializeField] AudioClip axe,
        beep1,
        beep2,
        beep3,
        beep4,
        checkPoint,
        enemyDead1,
        enemyDead2,
        enemyDead3,
        enemyHit,
        enemyShot1,
        enemyShot2,
        fountain,
        heal,
        jump,
        locking,
        pause,
        playerDead,
        playerHurt,
        playerShot,
        splash,
        sword1,
        sword2,
        sword3;
    //
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 
    //[SerializeField] AudioClip 

    static SFXManager instance;

    public static SFXManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        int objectCount = FindObjectsOfType<SFXManager>().Length;

        if (objectCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFX(SFXName name, float volume = 1)
    {
        AudioClip clip;
        switch (name)
        {
            case SFXName.Ammo:clip = ammo; break;
            case SFXName.Axe: clip = axe; break;
            case SFXName.Beep1: clip = beep1; break;
            case SFXName.Beep2: clip = beep2; break;
            case SFXName.Beep3: clip = beep3; break;
            case SFXName.Beep4: clip = beep4; break;
            case SFXName.CheckPoint: clip = checkPoint; break;
            case SFXName.EnemyDead1: clip = enemyDead1; break;
            case SFXName.EnemyDead2: clip = enemyDead2; break;
            case SFXName.EnemyDead3: clip = enemyDead3; break;
            case SFXName.EnemyHit: clip = enemyHit; break;
            case SFXName.EnemyShot1: clip = enemyShot1; break;
            case SFXName.EnemyShot2: clip = enemyShot2; break;
            case SFXName.Fountain: clip = fountain; break;
            case SFXName.Heal: clip = heal; break;
            case SFXName.Jump: clip = jump; break;
            case SFXName.Locking: clip = locking; break;
            case SFXName.Pause: clip = pause; break;
            case SFXName.PlayerDead: clip = playerDead; break;
            case SFXName.PlayerHurt: clip = playerHurt; break;
            case SFXName.PlayerShot: clip = playerShot; break;
            case SFXName.Splash: clip = splash; break;
            case SFXName.Sword1: clip = sword1; break;
            case SFXName.Sword2: clip = sword2; break;
            case SFXName.Sword3: clip = sword3; break;
            default: clip = beep1; break;
        }
        volume = Mathf.Clamp(volume, 0, 2);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public enum SFXName
    {
        Ammo,
        Axe,
        Beep1,
        Beep2,
        Beep3,
        Beep4,
        CheckPoint,
        EnemyDead1,
        EnemyDead2,
        EnemyDead3,
        EnemyHit,
        EnemyShot1,
        EnemyShot2,
        Fountain,
        Heal,
        Jump,
        Locking,
        Pause,
        PlayerDead,
        PlayerHurt,
        PlayerShot,
        Splash,
        Sword1,
        Sword2,
        Sword3,
    }
}
