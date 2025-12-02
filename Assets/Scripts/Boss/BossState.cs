public abstract class BossState
{
    // reference ke boss controllernya
    protected BossController bossController;

    // reference ke boss state machinenya
    protected BossStateMachine bossStateMachine;

    // constructornya
    public BossState(BossController bossController, BossStateMachine bossStateMachine)
    {
        this.bossController = bossController;
        this.bossStateMachine = bossStateMachine;
    }

    // blueprint buat setiap methodnya
    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void Exit() { }
}
