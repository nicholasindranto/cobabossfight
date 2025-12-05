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
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state, maka skip
        if (bossController.isOnCooldown) return;
    }
}
