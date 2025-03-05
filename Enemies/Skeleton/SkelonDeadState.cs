using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelonDeadState : EnemyState
{
    private Skeleton skeleton;
    public SkelonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton _skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.anim.SetBool(skeleton.lastAnimBoolName, true);
        skeleton.anim.speed = 0;
        skeleton.cd.enabled = false;
        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        if ( stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
