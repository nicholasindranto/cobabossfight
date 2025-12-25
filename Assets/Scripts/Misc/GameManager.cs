using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager instance;

    // apakah player lagi invincible
    public bool isPlayerInvincible = false;

    // reference ke playernya
    public Transform[] players;
    public int alivePlayerCount;

    // reference ke bossnya
    public Transform boss;

    // ui game win sama losenya
    public GameObject winUI;
    public GameObject loseUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // subscribe ke onplayerdiednya
        PlayerHealth.OnPlayerDied += HandlePlayerDied;
        BossHealth.OnBossDied += HandleBossDied;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDied;
        BossHealth.OnBossDied -= HandleBossDied;
    }

    private void HandlePlayerDied()
    {
        int temp = alivePlayerCount - 1;
        alivePlayerCount = Mathf.Clamp(temp, 0, temp);

        if (alivePlayerCount <= 0)
        {
            Debug.Log("GAMEOVER");

            // show ui nya
            loseUI.SetActive(true);
            winUI.SetActive(false);

            Time.timeScale = 0;
        }
    }

    private void HandleBossDied()
    {
        Debug.Log("GAMEOVER");

        // show ui nya
        loseUI.SetActive(false);
        winUI.SetActive(true);

        Time.timeScale = 0;
    }
}
