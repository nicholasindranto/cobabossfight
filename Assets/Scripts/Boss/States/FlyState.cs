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
    todo: implementasi fly state, bergerak ke posisi tertentu secara random dalam 3 kali perpindahan
    setelah itu baru kalau random.value < 0.5 maka ke walk state, kalau nggak ke attack 1
    */
}
