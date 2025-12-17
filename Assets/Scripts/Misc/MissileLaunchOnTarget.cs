using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MissileLaunchOnTarget : MonoBehaviour
{
    // reference lokasi missilenya
    public enum MissileSocket { None, Left, Right, Above }
    public MissileSocket socket = MissileSocket.None;

    // launch speednya
    public float launchSpeed;

    // durasi explosionnya
    public float explosionDuration;

    // referensi apakah udah selesai launch dan explode
    public bool isAlreadyDone = false;
    public bool GetIsAlreadyDone() => isAlreadyDone;

    // durasi idle nya
    public float idleDuration;
    // reference lagi idle apa nggaknya, by default nggak idle
    public bool isIdling = false;

    private void OnEnable()
    {
        // diawal pastikan false dulu
        isAlreadyDone = false;
        Attack3State.MissileLaunch += StartLaunchMissile;
    }

    private void OnDisable()
    {
        Attack3State.MissileLaunch -= StartLaunchMissile;
    }

    private void StartLaunchMissile(Vector3 finalM1, Vector3 finalM2, Vector3 finalM3)
    {
        // kalau lagi idle maka jangan launch lagi
        if (isIdling) return;

        switch (socket)
        {
            case MissileSocket.Left:
                if (GameManager.instance.player1) StartCoroutine(LaunchMissileCoroutine(finalM1));
                break;
            case MissileSocket.Right:
                if (GameManager.instance.player2) StartCoroutine(LaunchMissileCoroutine(finalM2));
                break;
            case MissileSocket.Above:
                StartCoroutine(LaunchPickUpMissileCoroutine(finalM3));
                break;
            default:
                Debug.LogError("TIDAK ADA POSISI MISSILE NYA!");
                break;
        }
    }

    private IEnumerator LaunchMissileCoroutine(Vector3 target)
    {
        // nggak jadi child dari socket boss nya lagi
        gameObject.transform.SetParent(null);

        // bikin biar missile langsung menghadap ke target
        // ambil duru arah alias directionnya kemana
        Vector3 direction = target - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = rot;

        while (Vector3.Distance(transform.position, target) > 0.01f) // selama belum nyampe
        {
            float step = launchSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }
        transform.position = target;

        // ambil colliderexplosionnya
        // lalu nyalain di target position
        GameObject explosionCollider = ExplosionPool.instance.GetFromPool();
        explosionCollider.transform.position = new Vector3(target.x, 0, target.z);
        explosionCollider.SetActive(true);

        // tunggu durasi explodenya
        yield return new WaitForSeconds(explosionDuration);
        // balikin ke pool
        ExplosionPool.instance.ReturnToPool(explosionCollider);

        // udah selesai
        isAlreadyDone = true;

        // kembalikan missilenya ke pool
        MissilePool.instance.ReturnToPool(gameObject, "regular");
    }

    private IEnumerator LaunchPickUpMissileCoroutine(Vector3 target)
    {
        // kalau lagi idle maka jangan launch lagi
        if (isIdling) yield return null;

        // nggak jadi child dari socket boss nya lagi
        gameObject.transform.SetParent(null);

        // bikin biar missile langsung menghadap ke target
        // ambil duru arah alias directionnya kemana
        Vector3 direction = target - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = rot;

        while (Vector3.Distance(transform.position, target) > 0.01f) // selama belum nyampe
        {
            float step = launchSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }

        // bikin biar berada di atas ground doang dan nggak miring
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        StartCoroutine(StartIdlingCoroutine());
    }

    private IEnumerator StartIdlingCoroutine()
    {
        // kalau lagi idle maka jangan launch lagi
        if (isIdling) yield return null;

        // lagi idle
        isIdling = true;
        Debug.LogWarning($"APAKAH MASUK dari {gameObject.name}");

        yield return new WaitForSeconds(idleDuration);

        // matiin isidlingnya dan balikin ke pool
        isIdling = false;
        MissilePool.instance.ReturnToPool(gameObject, "pickup");
    }
}
