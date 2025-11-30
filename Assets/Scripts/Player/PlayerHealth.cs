using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // jumlah healthnya
    [SerializeField] private float hp = 3f;

    // id player
    public int playerID;

    // reference ke ui player hp
    public TextMeshProUGUI player1HPText;
    public TextMeshProUGUI player2HPText;

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            TakeDamage(1);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            TakeDamage(2);
        }
    }

    public void TakeDamage(int id)
    {
        // kalau idnya beda, skip
        if (id != playerID) return;

        hp--;
        hp = Mathf.Clamp(hp, 0, hp);
        Debug.Log($"player {id} take damage");

        UpdateUI(id);

        if (hp <= 0)
        {
            Debug.Log($"player {id} dead");
            Destroy(gameObject);
        }
    }

    private void UpdateUI(int id)
    {
        if (id == 1) player1HPText.text = $"HP Player 1 = {hp}";
        else player2HPText.text = $"HP Player 2 = {hp}";
    }
}
