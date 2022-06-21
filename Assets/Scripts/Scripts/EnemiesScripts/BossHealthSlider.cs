using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthSlider : MonoBehaviour
{
    public static BossHealthSlider instance;

    public Slider bossHealthSlider;

    public Killable killObject;

    public BossBattle01 bossBattle;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bossHealthSlider.maxValue = killObject.CurrentHealth();
        bossHealthSlider.value = killObject.CurrentHealth();
    }

    public void UpdateHealthSlider(int healthPoints)
    {
        bossHealthSlider.value = healthPoints;
    }

}
