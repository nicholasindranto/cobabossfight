using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : BossState
{
    // constructor
    public FlyState(BossController bossController, BossStateMachine bossStateMachine) : base(bossController, bossStateMachine)
    {

    }

    /*
    rules :
    1. bisa ke attack1 / attack2 / attack3 state dengan percentage tertentu
    2. random berapa langkah
    */
    public override void Enter()
    {
        // nyalain transisi antar statenya
        bossController.StartCoroutine(bossController.CooldownBetweenTransitionState());

        // random berapa langkah
        bossController.moveToDo = Random.Range(bossController.flyStateMinMovementCount, (bossController.flyStateMaxMovementCount + 1));
        bossController.currentMoveCount = 0;

        // lagi transisi ke fly
        bossController.targetPos = new Vector3(bossController.transform.position.x, bossController.flyStateYPos, bossController.transform.position.z);
        bossController.StartCoroutine(bossController.TransitionTo("fly"));

        Debug.Log("flystate : entered");
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state atau lagi cooldown move, maka skip
        if (bossController.isOnCooldown || bossController.isOnMoveCooldown) return;

        // bikin gerak kesana
        bossController.Move();

        // kalau lagi transisi land atau fly atau kalau mau ke attack2, maka skip
        if (bossController.isTransitionLandFly) return;

        // kalau distance nya udah nyampe maka tambahin current move countnya
        if (Vector3.Distance(bossController.transform.position, bossController.targetPos) < 0.2f)
        {
            // kalau udah nyampe maka kasih jeda move cooldown
            bossController.StartCoroutine(bossController.MoveCooldown());

            bossController.currentMoveCount++;
            Debug.Log($"flystate : reach destination {bossController.currentMoveCount} / {bossController.moveToDo}");

            // masih bisa gerak random
            if (bossController.currentMoveCount < bossController.moveToDo)
            {
                float randValue = Random.value;
                // penentuan ke state attack1nya
                if (randValue < (bossController.flyStateToAttack1StatePercentage / 100f))
                {
                    // pindah ke attack1 state
                    bossStateMachine.ChangeState(bossController.attack1State);
                }
                else if (randValue < ((bossController.flyStateToAttack1StatePercentage + bossController.flyStateToAttack2StatePercentage) / 100f))
                {
                    // pindah ke attack2 state
                    bossStateMachine.ChangeState(bossController.attack2State);
                }
                else if (randValue < ((bossController.flyStateToAttack1StatePercentage + bossController.flyStateToAttack2StatePercentage + bossController.flyStateToAttack3StatePercentage) / 100f))
                {
                    // kpindah ke attack3 state
                    bossStateMachine.ChangeState(bossController.attack3State);
                }
                else
                {
                    // kalau nggak ya pindah ke fly around
                    bossController.SetNextDestination("fly");
                }
            }
            else
            {
                // ke attack1 statenya
                bossStateMachine.ChangeState(bossController.attack1State);
            }
        }
    }
}
