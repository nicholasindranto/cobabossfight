using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIndicator : MonoBehaviour
{
    // follownya berapa detik
    public float followTime;
    // detik sebelum boss slam
    public float timeBeforeSlam;
    // posisi terakhir track
    public Vector3 lastPosTrack = Vector3.zero;

    // method biar shadownya ngefollow player
    public IEnumerator FollowPlayer(Transform player)
    {
        // aktifkan gameobjectnya
        gameObject.SetActive(true);

        // set waktunya
        float currentTime = 0f;

        while (currentTime < followTime)
        {
            currentTime += Time.deltaTime;

            // set posisinya sama dengan si playernya di x dan z
            Vector3 newPos = new Vector3(player.position.x, 0.5f, player.position.z);
            transform.position = newPos;

            DoPulse();

            yield return null;
        }

        // cuma besar kecil dan nggak follow, tunggu sampai si boss nya turun lagi kebawah
        while (currentTime < (followTime + timeBeforeSlam))
        {
            currentTime += Time.deltaTime;
            DoPulse();
            yield return null;
        }

        // set posisi terakhir track nya
        lastPosTrack = transform.position;

        // matikan gameobjectnya
        gameObject.SetActive(false);
    }

    private void DoPulse()
    {
        // bikin juga efek membesar mengecilnya biar kaya efek "danger"
        // kenapa sin? karna biar dia kayak bergelombang gitu membesar mengecil
        // Time.time * 8f = biar 1 kali putara (membesar lalu mengecil) itu cepet 
        // sekitar 0.7 detik
        // dikali 0.15 dan ditambah 1 biar membesar 0.15 dan mengecil 0.15 (0.85 - 1.15)
        float pulse = 3f + Mathf.Sin(Time.time * 8f) * 0.15f;
        transform.localScale = Vector3.one * pulse;
    }
}
