using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimShockWaveState : PlayerState
{
    
    public PlayerAimShockWaveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.RSWSkill.DotActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.RSWSkill.DotActive(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            player.BusyFor(0.5f);
            stateMachine.ChangeState(player.idleState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if(player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }
}
