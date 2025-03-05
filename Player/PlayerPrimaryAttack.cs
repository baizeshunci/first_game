using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public int comboCounter {  get; private set; } 
    private float lastTimeAttacked;
    private float comboWindow = 2;


    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time>= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * player.facingDir, player.attackMovement[comboCounter].y);

        player.anim.SetInteger("ComboCounter",comboCounter);
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
        if (comboCounter == 0)
        {
            player.StartCoroutine("BusyFor", 0.1f);
        }
        else if (comboCounter == 1)
        {
            player.StartCoroutine("BusyFor", .15f);
        }
        else if (comboCounter == 2)
        {
            player.StartCoroutine("BusyFor", .2f);
        }


        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
