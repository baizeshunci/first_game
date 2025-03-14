using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected string animBoolName;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    protected float stateTimer;


    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        if (enemyBase == null)
        {
            //Debug.Log("Enemy is null in EnemyState constructor!");
        }
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        rb = enemyBase.rb;
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName,true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
