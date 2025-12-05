using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BossState
{
    // constructor
    public WalkState(BossController bossController, BossStateMachine bossStateMachine) : base(bossController, bossStateMachine)
    {

    }

    /*
    rules :
    1. sekian% bakalan ke flystate, sisanya ke attack1
    2. random jumlah movenya
    3. sekian% ke attack1 state, sisanya tetep move
    */
    public override void Enter()
    {
        // nyalain transisi antar statenya
        bossController.StartCoroutine(bossController.CooldownBetweenTransitionState());

        // set semua variabelnya
        bossController.moveToDo = Random.Range(bossController.walkStateMinMovementCount, (bossController.walkStateMaxMovementCount + 1));
        bossController.currentMoveCount = 0;

        // lagi transisi ke walk
        bossController.targetPos = new Vector3(bossController.transform.position.x, bossController.walkStateYPos, bossController.transform.position.z);
        bossController.StartCoroutine(bossController.TransitionTo("walk"));

        Debug.Log("walkstate : entered");
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state atau lagi cooldown move, maka skip
        if (bossController.isOnCooldown || bossController.isOnMoveCooldown) return;

        // bikin dia menuju kesana
        bossController.Move();

        // kalau lagi transisi land atau fly, maka skip
        if (bossController.isTransitionLandFly) return;

        // kalau distance nya udah nyampe maka tambahin current move countnya
        if (Vector3.Distance(bossController.transform.position, bossController.targetPos) < 0.2f)
        {
            // kalau udaah nyampe maka tunggu dulu sebelum dia gerak lagi
            bossController.StartCoroutine(bossController.MoveCooldown());

            bossController.currentMoveCount++;
            Debug.Log($"walkstate : reach destination {bossController.currentMoveCount} / {bossController.moveToDo}");

            // masih bisa gerak random
            if (bossController.currentMoveCount < bossController.moveToDo)
            {
                // penentuan ke state attacknya
                if (Random.value < (bossController.walkStateToAttack1StatePercentage / 100f))
                {
                    // pindah ke attack1 state
                    bossStateMachine.ChangeState(bossController.attack1State);
                }
                else
                {
                    // kalau nggak ya pindah lagi ke lokasi lain
                    bossController.SetNextDestination("walkstate");
                }
            }
            else
            {
                // ke fly statenya
                bossStateMachine.ChangeState(bossController.flyState);
            }
        }
    }
}
