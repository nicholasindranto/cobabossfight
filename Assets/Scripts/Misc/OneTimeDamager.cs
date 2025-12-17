using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeDamager : MonoBehaviour
{
    // sudah kena damage atau belum?
    [SerializeField] private bool isPlayer1TakeDamage = false;
    [SerializeField] private bool isPlayer2TakeDamage = false;

    private void OnDisable()
    {
        // kalau udah nggak aktif, maka reset semuanya
        isPlayer1TakeDamage = false;
        isPlayer2TakeDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // kalau ndak bisa ngasih damake (lagi warmup) atau udah terkena damage maka skip
        if (GameManager.instance.isPlayerInvincible) return;

        if (other.CompareTag("Player1"))
        {
            if (!isPlayer1TakeDamage) // kalau belum terkena damage maka kasih damage
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
                isPlayer1TakeDamage = true;
            }
        }

        if (other.CompareTag("Player2"))
        {
            if (!isPlayer2TakeDamage) // kalau belum terkena damage maka kasih damage
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(2);
                isPlayer2TakeDamage = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // false kan semuanya
        if (other.CompareTag("Player1")) isPlayer1TakeDamage = false;
        if (other.CompareTag("Player2")) isPlayer2TakeDamage = false;
    }
}
