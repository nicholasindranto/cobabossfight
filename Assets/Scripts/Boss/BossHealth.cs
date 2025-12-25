using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BossHealth : MonoBehaviour
{
    // jumlah health nya
    [SerializeField] private float hp = 3f;

    public TextMeshProUGUI bossHPUI;

    // event boss died
    public static event Action OnBossDied;

    public void TakeDamage()
    {
        float temp = hp - 1;
        hp = Mathf.Clamp(hp, 0, temp);
        Debug.Log("boss take damage from pickupble missile");

        UpdateUI();

        if (hp <= 0)
        {
            Debug.Log("boss dead, player win");
            // invoke event boss diednya
            OnBossDied?.Invoke();
            Destroy(gameObject);
        }
    }

    private void UpdateUI() => bossHPUI.text = $"HP Boss = {hp}";
}
