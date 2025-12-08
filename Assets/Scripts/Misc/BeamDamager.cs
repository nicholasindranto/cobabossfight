using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDamager : MonoBehaviour
{
    // lagi ada di dalam collider nggak
    private bool isPlayerInside = false;

    // sudah kena damage atau belum?
    private bool isDamageTaken = false;

    private void OnTriggerEnter(Collider other)
    {
        isPlayerInside = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // kalau ndak bisa ngasih damake (lagi warmup) atau udah terkena damage maka skip
        if (GameManager.instance.isPlayerInvincible || isDamageTaken) return;

        if (isPlayerInside)
        {
            if (other.CompareTag("Player1")) other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            else if (other.CompareTag("Player2")) other.gameObject.GetComponent<PlayerHealth>().TakeDamage(2);

            isDamageTaken = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        isDamageTaken = false;
    }
}
