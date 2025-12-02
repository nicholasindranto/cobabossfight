using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // reference ke bossstatemachinenya
    public BossStateMachine stateMachine { get; private set; }

    // semua statenya
    private WalkState walkState;
    private FlyState flyState;

    private void Awake()
    {
        // bikin object statemachinenya
        stateMachine = new BossStateMachine();

        // bikin juga setiap statenya
        walkState = new WalkState(this, stateMachine);
        flyState = new FlyState(this, stateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        // inisialisasi statenya ke walk
        stateMachine.Initialize(walkState);
    }

    // Update is called once per frame
    void Update()
    {
        // jalanin logika update statemachinenya
        stateMachine.Update();
    }
}
