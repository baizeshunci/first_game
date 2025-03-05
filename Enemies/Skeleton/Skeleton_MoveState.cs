using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_MoveState : SkeletonGroundState
{
    public Skeleton_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton _skeleton) : base(_enemyBase, _stateMachine, _animBoolName, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemyBase.SetVelocity(skeleton.moveSpeed * skeleton.facingDir, skeleton.rb.velocity.y);

        if (!skeleton.IsGroundDectected() || skeleton.IsWallDectected())
        {
            if (!skeleton.isKnocked)
            {
                skeleton.Flip();
                stateMachine.ChangeState(skeleton.idleState);
            }
        }
    }
}
