using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchShockWavesState : PlayerState
{
    private Transform RCShockWave;
    public PlayerCatchShockWavesState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.RSWSkill.coolDownRecord = Time.time;

        RCShockWave = player.RCShockWave.transform;
        rb.velocity = new Vector2( player.RCShockWaveReturnImpact * -player.facingDir,rb.velocity.y);

        if (player.transform.position.x > RCShockWave.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < RCShockWave.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
        PlayerManager.instance.player.RCShockWaveTime = 0;
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Exit()
    {
        base.Exit();
        
        PlayerManager.instance.player.RCShockWaveTime = 0;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
