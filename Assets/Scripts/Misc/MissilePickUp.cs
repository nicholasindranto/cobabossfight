using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : MonoBehaviour
{
    public bool readyToShoot = false;
    // bisa memberikan damage apa nggak
    public bool canDamageBoss = false;

    // lagi dipicked apa nggak
    public bool isPickedUp = false;

    private void OnDisable()
    {
        // ketika di disable maka reset jadi false lagi
        readyToShoot = false;

        canDamageBoss = false;

        isPickedUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // ambil script missile launch nya dulu buat menonaktifkan coroutinenya
            MissileLaunchOnTarget script = GetComponentInParent<MissileLaunchOnTarget>();

            if (script != null)
            {
                // stop countdownnya
                script.CancelIdleCountdown();
                // di pickup sama player
                isPickedUp = true;
            }
            else Debug.LogError("nggak ada scriptnya");

            // setelah itu baru set parentnya ke si player biar dia ngikutin player
            script.transform.SetParent(other.transform);
            script.transform.localPosition = Vector3.up * 3;
            script.transform.localEulerAngles = Vector3.zero;

            // udah ready to shoot
            readyToShoot = true;
        }
        else if (other.CompareTag("Boss"))
        {
            Debug.LogWarning("apakah masuk?");
            // kalau udah siap menjadi missile yang dishoot oleh player, maka kasih damage
            // kalau nggak maka skip
            if (!canDamageBoss) return;
            Debug.LogWarning("apakah nyampe sini?");
            // kasi damage ke boss
            other.GetComponent<BossHealth>().TakeDamage();
        }
    }
}
