using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Skeleton_IdleState : SkeletonGroundState
{
    public Skeleton_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton _skeleton) : base(_enemyBase, _stateMachine, _animBoolName, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(skeleton.moveState);
        }
        
    }
}
