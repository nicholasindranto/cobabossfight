using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3State : BossState
{
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
        bossController.StartCoroutine(bossController.DebugAttackState(2, "attack 3", bossController.walkState));
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state, maka skip
        if (bossController.isOnCooldown) return;
    }

    public override void Exit()
    {
        // pastikan di akhir chain attack tidak ada state lagi
        bossController.DequeueStateAfterTransition();
    }
}
