using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(xInput != 0)
            player.SetVelocity(xInput * player.movespeed * 0.8f, rb.velocity.y);
    }
}
