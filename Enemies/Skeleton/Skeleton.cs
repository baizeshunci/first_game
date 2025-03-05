using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class Skeleton : Enemy
{
    #region States
    public Skeleton_IdleState idleState {  get; private set; }
    
    public Skeleton_MoveState moveState { get; private set; }
    
    public SkeletonBattleState battleState { get; private set; }

    public SkeletonAttackState attackState { get; private set; }

    public SkeletonStunnedState stunnedState { get; private set; }

    public SkelonDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        
        idleState = new Skeleton_IdleState(this, stateMachine, "Idle", this);

        moveState = new Skeleton_MoveState(this, stateMachine, "Move", this);

        battleState = new SkeletonBattleState(this,stateMachine, "Move",this);

        attackState = new SkeletonAttackState(this, stateMachine, "Attack",this);

        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);

        deadState = new SkelonDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();


    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
