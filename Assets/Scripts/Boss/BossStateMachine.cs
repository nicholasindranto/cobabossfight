using UnityEngine;

public class BossStateMachine
{
    // state sekarang
    public BossState currentState { get; private set; }

    // inisialisasi statenya
    public void Initialize(BossState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    // buat ngubah statenya
    public void ChangeState(BossState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }

    // buat jalanin logika updatenya
    public void Update()
    {
        currentState.LogicUpdate();
    }
}
