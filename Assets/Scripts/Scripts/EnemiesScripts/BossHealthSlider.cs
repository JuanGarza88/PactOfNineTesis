using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthSlider : MonoBehaviour
{
    public static BossHealthSlider instance;

    public Slider bossHealthSlider;

    public Killable bossHealth;
    public bool isDamageable;

    public BossBattle01 bossBattle;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isDamageable = true;
        bossHealthSlider.maxValue = bossHealth.CurrentHealth();
        bossHealthSlider.value = bossHealth.CurrentHealth();
    }

    public void UpdateHealthSlider(int healthPoints)
    {
        bossHealthSlider.value = healthPoints;
    }

}
