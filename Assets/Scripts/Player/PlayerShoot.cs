using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float missileSpeed;

    public void Shoot(InputAction.CallbackContext context)
    {
        // kalau ndak diteken maka skip
        if (!context.started) return;

        // ambil dulu missilepickup scriptnya dulu untuk mendeteksi apakah udah siap nembak apa belum
        MissilePickUp script = GetComponentInChildren<MissilePickUp>();
        if (script != null)
        {
            // kalau belum siap buat nembak maka skip juga
            if (!script.readyToShoot) return;

            // set parent nya ke world biasa
            script.transform.parent.SetParent(null);

            // start coroutine buat nembak ke arah si boss
            StartCoroutine(ShootAtBossCoroutine(script));
        }
        else Debug.LogError("tidak ketemu scriptnya");
    }

    private IEnumerator ShootAtBossCoroutine(MissilePickUp script)
    {
        // gameobject missilenya
        GameObject missilePickUp = script.transform.parent.gameObject;

        // rotasi biar langsung menghadap ke bossnya
        Vector3 targetPos = GameManager.instance.boss.position;

        // rotasi ke bossnya
        Vector3 direction = targetPos - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        missilePickUp.transform.rotation = rot;

        // set bisa memberikan damage ke boss
        script.canDamageBoss = true;

        // launch menuju ke boss
        while (Vector3.Distance(missilePickUp.transform.position, targetPos) > 0.01f) // selama belum nyampe
        {
            float step = missileSpeed * Time.deltaTime;
            missilePickUp.transform.position = Vector3.MoveTowards(missilePickUp.transform.position, targetPos, step);
            yield return null;
        }

        // kalau udah maka kasih damage ke boss dan balikin ke pool tapi tunggu bentar dulu
        // yield return new WaitForSeconds(0.1f);
        MissilePool.instance.ReturnToPool(missilePickUp, "pickup");
    }
}
