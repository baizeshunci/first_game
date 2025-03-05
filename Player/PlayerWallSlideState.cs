using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public float dir;

    public override void Enter()
    {
        base.Enter();
        dir = player.facingDir;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (xInput != 0 && dir != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (yInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y * 0.9f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.playerWallJump);
            return;
        }
        if (!player.IsWallDectected() && !player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.fallState);
        }
        if(player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
