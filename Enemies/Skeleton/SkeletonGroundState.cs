using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected Skeleton skeleton;

    protected Transform player;
    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Skeleton _skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetedted() || Vector2.Distance(skeleton.transform.position,player.position) < 2 )
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
        
    }
}
