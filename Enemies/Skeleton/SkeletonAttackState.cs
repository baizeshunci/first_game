using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    
    private Skeleton skeleton;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.lastTimeAttack = Time.time;
    }
    public override void Update()
    {
        base.Update();

        skeleton.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
    }

    
}
