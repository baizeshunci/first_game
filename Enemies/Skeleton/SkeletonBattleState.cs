using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkeletonBattleState : SkeletonGroundState
{
    private int moveDir;
    private Transform player1;
    private float error;

    
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton _skeleton) : base(_enemyBase, _stateMachine, _animBoolName, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.battleTime;
        player1 = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetedted())
        {
            stateTimer = skeleton.battleTime;

            if (skeleton.IsPlayerDetedted().distance <= skeleton.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(skeleton.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player1.transform.position , skeleton.transform.position) > 7)
            {
                stateMachine.ChangeState(skeleton.idleState);
            }
        }

        error = player.position.x - skeleton.transform.position.x;

        if (error >= 0.5f)
        {
            moveDir = 1;
        }
        else if (error <= -0.5f) 
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }

        if (moveDir == 0 && !CanAttack())
        {
            stateMachine.ChangeState(skeleton.idleState);
        }

        skeleton.SetVelocity(2 * skeleton.moveSpeed * moveDir,rb.velocity.y);
    }


    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time >= skeleton.lastTimeAttack+skeleton.attackCooldown)
        {
            return true;
        }
        return false;
    }

    
}
