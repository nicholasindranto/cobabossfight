using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLockOnTarget : MonoBehaviour
{
    // reference lokasi missilenya
    public enum MissileSocket { None, Left, Right, Above }
    public MissileSocket socket = MissileSocket.None;

    // berapa lama nge lock playernya
    public float lockTime = 1f;
    public float missileRotationSpeed = 5f;

    // final targetnya setelah lock on
    public Vector3 finalTarget = Vector3.zero;

    private void OnEnable()
    {
        // subscribe ke event missile lock on nya
        Attack3State.MissileLockOn += StartLockOn;
    }

    private void OnDisable()
    {
        Attack3State.MissileLockOn -= StartLockOn;
    }

    private void StartLockOn(Transform p1, Transform p2, Vector3 randPos)
    {
        // panggil coroutinenya
        switch (socket)
        {
            case MissileSocket.Left:
                if (p1) StartCoroutine(LockOnPlayerCoroutine(p1));
                break;
            case MissileSocket.Right:
                if (p2) StartCoroutine(LockOnPlayerCoroutine(p2));
                break;
            case MissileSocket.Above:
                StartCoroutine(LockOnRandomCoroutine(randPos));
                break;
            default:
                Debug.LogError("TIDAK ADA POSISI MISSILE NYA!");
                break;
        }
    }

    private IEnumerator LockOnPlayerCoroutine(Transform target)
    {
        // starttimenya dari 0
        float startTimeMissile = 0f;
        while (startTimeMissile < lockTime)
        {
            // ambil arah dari target player2 ke missile nya
            // yang rotasi x sama z nya, y nya tetap
            Vector3 direction = target.position - transform.position;
            direction.y = 0f; // lock di y axis nya

            // target rotasinya mau ke directionnya itu sendiri
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // rotasikan missile nya ke si target
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                missileRotationSpeed * Time.deltaTime
            );

            // tambahin timenya
            startTimeMissile += Time.deltaTime;

            yield return null;
        }

        // set final targetnya
        finalTarget = target.position;
    }

    private IEnumerator LockOnRandomCoroutine(Vector3 randPos)
    {
        // kalau lagi idling maka jangan jalankan
        if (GetComponent<MissileLaunchOnTarget>().isIdling) yield return null;

        // missile 3 random targetnya
        float startTimeMissile = 0f;

        while (startTimeMissile < lockTime)
        {
            Vector3 direction = randPos - transform.position;
            direction.y = 0f; // lock di y axis nya

            // target rotasinya mau ke directionnya itu sendiri
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // rotasikan missile 3 nya ke direction randomnya
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                missileRotationSpeed * Time.deltaTime
            );

            // tambahin timenya
            startTimeMissile += Time.deltaTime;

            yield return null;
        }

        // set final targetnya
        finalTarget = randPos;
    }
}
