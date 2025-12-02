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
    todo: implementasi walk state, bergerak ke posisi tertentu secara random dalam 3 kali perpindahan
    setelah itu baru kalau random.value < 0.5 maka ke fly state, kalau nggak ke attack 1
    */
}
