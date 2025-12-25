using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Attack3State : BossState
{
    // event driven
    public static event Action<Transform, Transform, Vector3> MissileLockOn;
    public static event Action<Vector3, Vector3, Vector3> MissileLaunch;

    // constructornya
    public Attack3State(BossController bossController, BossStateMachine bossStateMachine) : base(bossController, bossStateMachine)
    {

    }

    /*
    todo: rocket launch 3 rocket at targeted player, 2 exploded, 1 remains stationary, pickup-able
    */
    public override void Enter()
    {
        // nyalain transisi antar statenya
        bossController.StartCoroutine(bossController.CooldownBetweenTransitionState());

        Debug.Log("attack3state : entered");

        // langsung spawn missilenya di spot bossnya
        // bikin missile 1 dan 2 nya lock player sama lock random target di missile 3 nya
        bossController.StartCoroutine(SpawnMissileAndLaunch());
    }
    public override void Exit()
    {
        // pastikan di akhir chain attack tidak ada state lagi
        bossController.DequeueStateAfterTransition();
    }

    private IEnumerator SpawnMissileAndLaunch()
    {
        GameObject missile1 = null;
        GameObject missile2 = null;
        GameObject missile3 = MissilePool.instance.GetFromPool("pickup");
        Debug.LogWarning($"apakah missile 3 ada = {missile3}");

        // buat missile 1
        if (GameManager.instance.players[0] != null)
        {
            // ambil dulu dari pool nya
            missile1 = MissilePool.instance.GetFromPool("regular");

            // set socketnya
            // ambil dulu script missilelockontargetnya dulu
            MissileLockOnTarget m1script = missile1.GetComponent<MissileLockOnTarget>();
            m1script.socket = MissileLockOnTarget.MissileSocket.Left;
            MissileLaunchOnTarget m1launchscript = missile1.GetComponent<MissileLaunchOnTarget>();
            m1launchscript.socket = MissileLaunchOnTarget.MissileSocket.Left;

            // set parentnya ke setiap socketnya
            missile1.transform.SetParent(bossController.leftSocket);

            // reset posisi dan rotasi dari setiap missile
            missile1.transform.localPosition = Vector3.zero;

            // nyalain semua missile biar dia ngetarget
            missile1.SetActive(true);
        }

        if (GameManager.instance.players[1] != null)
        {
            missile2 = MissilePool.instance.GetFromPool("regular");

            MissileLockOnTarget m2script = missile2.GetComponent<MissileLockOnTarget>();
            m2script.socket = MissileLockOnTarget.MissileSocket.Right;
            MissileLaunchOnTarget m2launchscript = missile2.GetComponent<MissileLaunchOnTarget>();
            m2launchscript.socket = MissileLaunchOnTarget.MissileSocket.Right;

            missile2.transform.SetParent(bossController.rightSocket);

            missile2.transform.localPosition = Vector3.zero;

            missile2.SetActive(true);
        }

        MissileLockOnTarget m3script = missile3.GetComponent<MissileLockOnTarget>();
        Debug.LogWarning($"apakah ada script lock on nya = {m3script}");
        m3script.socket = MissileLockOnTarget.MissileSocket.Above;
        MissileLaunchOnTarget m3launchscript = missile3.GetComponent<MissileLaunchOnTarget>();
        m3launchscript.socket = MissileLaunchOnTarget.MissileSocket.Above;

        missile3.transform.SetParent(bossController.aboveSocket);

        missile3.transform.localPosition = Vector3.zero;

        // nyalain target ui nya player
        if (bossController.targetUIPlayer1) bossController.targetUIPlayer1.SetActive(true);
        if (bossController.targetUIPlayer2) bossController.targetUIPlayer2.SetActive(true);

        missile3.SetActive(true);

        // invoke actionnya untuk lock on target missilenya
        Vector3 randPos = new Vector3(
            UnityEngine.Random.Range(bossController.topLeftCornerPos.position.x, bossController.bottomRightPos.position.x),
            0.5f,
            UnityEngine.Random.Range(bossController.topLeftCornerPos.position.z, bossController.bottomRightPos.position.z)
        );
        MissileLockOn?.Invoke(GameManager.instance.players[0], GameManager.instance.players[1], randPos);

        // tunggu missilenya masih lock on
        yield return new WaitForSeconds(missile1.GetComponent<MissileLockOnTarget>().lockTime);

        // matikan lagi ui nya karna udah gak lock on
        if (bossController.targetUIPlayer1) bossController.targetUIPlayer1.SetActive(false);
        if (bossController.targetUIPlayer2) bossController.targetUIPlayer2.SetActive(false);

        // tunggu dulu beberapa saat
        yield return new WaitForSeconds(bossController.waitUntilLaunchTime);

        // setelah itu baru langsung launch ke targetnya
        // invoke lagi biar launch nya barengan
        // ambil final targetnya dulu
        Vector3 finalTargetM1 = Vector3.zero;
        if (GameManager.instance.players[0]) finalTargetM1 = missile1.GetComponent<MissileLockOnTarget>().finalTarget;
        Vector3 finalTargetM2 = Vector3.zero;
        if (GameManager.instance.players[1]) finalTargetM2 = missile2.GetComponent<MissileLockOnTarget>().finalTarget;
        Vector3 finalTargetM3 = missile3.GetComponent<MissileLockOnTarget>().finalTarget;
        MissileLaunch?.Invoke(finalTargetM1, finalTargetM2, finalTargetM3);

        // tunggu sampai udah selesai explode sama launch
        yield return new WaitUntil(missile1.GetComponent<MissileLaunchOnTarget>().GetIsAlreadyDone);

        // setelah selesai baru pindah ke walk state lagi
        bossStateMachine.ChangeState(bossController.walkState);
    }
}
