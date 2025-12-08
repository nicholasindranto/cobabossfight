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
        int randPlayer = Random.Range(1, 3);
        if (randPlayer == 1) targetPlayer = GameManager.instance.player1;
        else targetPlayer = GameManager.instance.player2;
    }

    public override void LogicUpdate()
    {
        // kalau lagi transisi antar state, maka skip
        if (bossController.isOnCooldown) return;
    }
}
