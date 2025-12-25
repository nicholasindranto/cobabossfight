using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : BossState
{
    private Transform targetPlayer = null;

    // constructornya
    public Attack2State(BossController bossController, BossStateMachine bossStateMachine) : base(bossController, bossStateMachine)
    {

    }

    // todo: eagle strike slam down to target player
    public override void Enter()
    {
        // nyalain transisi antar statenya
        bossController.StartCoroutine(bossController.CooldownBetweenTransitionState());

        Debug.Log("attack2state : entered");
        // bossController.StartCoroutine(bossController.DebugAttackState(2, "attack 2", bossController.attack3State));

        // set random target playernya dulu
        if (GameManager.instance.players[0] != null && GameManager.instance.players[1] != null) targetPlayer = GameManager.instance.players[Random.Range(0, 2)];
        else if (GameManager.instance.players[0] == null && GameManager.instance.players[1] != null) targetPlayer = GameManager.instance.players[1];
        else if (GameManager.instance.players[0] != null && GameManager.instance.players[1] == null) targetPlayer = GameManager.instance.players[0];
        else targetPlayer = null;

        // bikin boss nya keatas (terbang keatas)
        bossController.StartCoroutine(GoingUp());

        // shadownya ngikutin
        // kalau nggak ada target playernya maka skip
        if (targetPlayer != null) bossController.StartCoroutine(bossController.shadowPrefab.GetComponent<ShadowIndicator>().FollowPlayer(targetPlayer));

        // bossnya teleport ke posisi terakhir setelah nunggu tracking shadow dan slam
        bossController.StartCoroutine(WaitAndSlam());
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state, maka skip
        if (bossController.isOnCooldown) return;
    }

    private IEnumerator GoingUp()
    {
        // kecepatan verticalnya
        float speed = bossController.verticalSpeed;

        // posisi start = posisi saat ini
        Vector3 startPos = bossController.transform.position;
        Vector3 endPos = new Vector3(startPos.x, bossController.offScreenY, startPos.z);
        // loop sampai mendekati ke end pos nya
        while (Vector3.Distance(bossController.transform.position, endPos) > 0.01f)
        {
            // maka keatas
            float step = speed * Time.deltaTime;
            bossController.transform.position = Vector3.MoveTowards(bossController.transform.position, endPos, step);
            yield return null;
        }

        // set lokasi tepatnya setelah itu keluar
        bossController.transform.position = endPos;
        yield break;
    }

    private IEnumerator WaitAndSlam()
    {
        // ambil shadowindicatornya dulu
        ShadowIndicator siScript = bossController.shadowPrefab.GetComponent<ShadowIndicator>();
        // tunggu sampai sudah selesai ngetrack
        yield return new WaitForSeconds(siScript.followTime + siScript.timeBeforeSlam + 0.05f);

        // pindahin bossnya di atas target positionnya
        Vector3 above = new Vector3(siScript.lastPosTrack.x, bossController.offScreenY, siScript.lastPosTrack.z);
        bossController.transform.position = above;

        // ambil final positionnya
        Vector3 finalPos = new Vector3(siScript.lastPosTrack.x, bossController.walkStateYPos, siScript.lastPosTrack.z);

        // kecepatan verticalnya
        float speed = bossController.verticalSpeed;

        // aktifkan dulu collider splash damage nya
        bossController.splashDamageCollider.SetActive(true);

        // slam dengan cepet kaya pas keatas
        while (Vector3.Distance(bossController.transform.position, finalPos) > 0.01f)
        {
            // maka keatas
            float step = speed * Time.deltaTime;
            bossController.transform.position = Vector3.MoveTowards(bossController.transform.position, finalPos, step);
            yield return null;
        }
        // set posisi akhirnya
        bossController.transform.position = finalPos;

        // tunggu sebentar lagi biar splash damagenya beneran kerasa efeknya
        yield return new WaitForSeconds(0.5f);

        // matikan lagi collider splash damage nya
        bossController.splashDamageCollider.SetActive(false);

        // lanjut ke state attack 3
        bossStateMachine.ChangeState(bossController.attack3State);
    }
}