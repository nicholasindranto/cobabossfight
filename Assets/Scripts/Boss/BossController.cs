using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Common Settings")]
    public Transform topLeftCornerPos;
    public Transform bottomRightPos;
    public bool isTransitionLandFly = false;
    public float transitionCooldown;
    public bool isOnCooldown = false;
    public bool isOnMoveCooldown = false;
    public float moveCooldown;

    [Header("Walk State Settings")]
    public int walkStateMinMovementCount;
    public int walkStateMaxMovementCount;
    public float walkStateToAttack1StatePercentage;
    public float walkStateSmoothMovement;
    public float walkStateYPos;

    [Header("Fly State Settings")]
    public int flyStateMinMovementCount;
    public int flyStateMaxMovementCount;
    public float flyStateSmoothMovement;
    public float flyStateYPos;
    public float flyStateToAttack1StatePercentage;
    public float flyStateToAttack2StatePercentage;
    public float flyStateToAttack3StatePercentage;

    [Header("Attack 1 Settings")]
    public GameObject beamCollider;

    // reference ke bossstatemachinenya
    public BossStateMachine stateMachine { get; private set; }

    // semua statenya
    public WalkState walkState;
    public FlyState flyState;
    public Attack1State attack1State;
    public Attack2State attack2State;
    public Attack3State attack3State;

    public int moveToDo;
    public int currentMoveCount;
    public Vector3 targetPos;

    private void Awake()
    {
        // bikin object statemachinenya
        stateMachine = new BossStateMachine();

        // bikin juga setiap statenya
        walkState = new WalkState(this, stateMachine);
        flyState = new FlyState(this, stateMachine);
        attack1State = new Attack1State(this, stateMachine);
        attack2State = new Attack2State(this, stateMachine);
        attack3State = new Attack3State(this, stateMachine);
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

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, walkStateSmoothMovement);
    }

    public void SetNextDestination(string debugFromState)
    {
        // set random posisi x z nya
        float xPos = Random.Range(topLeftCornerPos.position.x, bottomRightPos.position.x);
        float zPos = Random.Range(topLeftCornerPos.position.z, bottomRightPos.position.z);

        // update target pos nya
        targetPos = new Vector3(xPos, targetPos.y, zPos);

        Debug.Log($"{debugFromState} : next location = {targetPos}");
    }

    public IEnumerator DebugAttackState(int waitTime, string fromState, BossState changeToState)
    {
        Debug.Log($"attack using state : {fromState} for {waitTime} seconds");
        yield return new WaitForSeconds(waitTime);
        stateMachine.ChangeState(changeToState);
    }

    public IEnumerator TransitionTo(string stateName)
    {
        // lagi transisi
        isTransitionLandFly = true;

        // tunggu sampai dia nyampe ke target pos nya
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPos) < 0.2f);

        // setelah nyampe false lagi transisinya
        isTransitionLandFly = false;
    }

    public IEnumerator CooldownBetweenTransitionState()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(transitionCooldown);

        isOnCooldown = false;
    }

    public IEnumerator MoveCooldown()
    {
        isOnMoveCooldown = true;

        yield return new WaitForSeconds(moveCooldown);

        isOnMoveCooldown = false;
    }

    // cek apakah di ground apa nggak
    public bool IsGrounded() => (transform.position.y <= (walkStateYPos + 0.1f)) && (transform.position.y >= (walkStateYPos - 0.1f));
}
