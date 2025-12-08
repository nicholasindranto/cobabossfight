using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : BossState
{
    // constructornya
    public Attack1State(BossController bossController, BossStateMachine bossStateMachine) : base(bossController, bossStateMachine)
    {

    }

    /*
    todo: fire flame state straight line vertical beam
    */
    public override void Enter()
    {
        // nyalain transisi antar statenya
        bossController.StartCoroutine(bossController.CooldownBetweenTransitionState());

        Debug.Log("attack1state : entered");

        // startcoroutine buat beam nya
        bossController.StartCoroutine(Attack1Coroutine());
    }

    private IEnumerator Attack1Coroutine()
    {
        // set lokasi spawn x dan z nya
        // x dibikin sama dengan posisi boss nya
        // y sama dengan posisi y prefab nya
        // z dibikin sama lalu dikurangi dengan offset nya biar dia didepan boss
        float spawnZ = bossController.transform.position.z - bossController.spawnZOffset;

        Vector3 spawnPos = new Vector3(bossController.transform.position.x, bossController.beamCollider.transform.position.y, spawnZ);

        // ambil dari poolnya
        GameObject beamCollObj = BeamPool.instance.GetFromPool();

        // set lokasinya dan aktifkan
        beamCollObj.transform.position = spawnPos;
        beamCollObj.SetActive(true);

        // warm up dulu, jangan kasi damage ke player
        beamCollObj.GetComponent<MeshRenderer>().material.color = Color.green; // buat debug aja
        GameManager.instance.isPlayerInvincible = true;
        yield return new WaitForSeconds(bossController.warmUpDuration);
        GameManager.instance.isPlayerInvincible = false;

        // lalu tunggu biar beamnya aktif terus selama beberapa detik
        beamCollObj.GetComponent<MeshRenderer>().material.color = Color.red; // buat debug aja
        yield return new WaitForSeconds(bossController.beamDuration);

        // udah selesai maka balikin ke pool
        BeamPool.instance.ReturnToPool(beamCollObj);

        // tentukan ke next state nya
        // kalau lagi di tanah, maka masukin state attack2 ke queue lalu pindah ke fly
        if (bossController.IsGrounded())
        {
            bossController.EnqueueStateAfterTransition(bossController.attack2State);
            bossStateMachine.ChangeState(bossController.flyState);
            yield return null;
        }

        // kalau lagi di udara, maka langsung ke attack2
        bossStateMachine.ChangeState(bossController.attack2State);
    }
}
