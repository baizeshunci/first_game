using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (SkillManager.instance.RSWSkill.coolDown + SkillManager.instance.RSWSkill.coolDownRecord < Time.time)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && HasRCShockWave() && PlayerManager.instance.player.RCShockWaveTime == 0 && player.skill.RSWSkill.recircleUnlocked)
            {
                PlayerManager.instance.player.RCShockWaveTime = 1;
                stateMachine.ChangeState(player.aimShockWave);
            }
        }

        if(player == null)
        {
            Debug.Log("Player is null");
        }
        else if(player.skill == null)
        {
            Debug.Log("Player skill is null");
        }
        else if(player.skill.parry == null)
        {
            Debug.Log("Player parry is null");
        }

        if(Input.GetKeyDown(KeyCode.R) && player.skill.blankhole.blackholeUnlocked)
        {
            PlayerManager.instance.player.dashState.canDash = false;
            stateMachine.ChangeState(player.blankholeState);
        }
        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttack);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }
        if (!player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
    public bool HasRCShockWave()
    {
        if (!player.RCShockWave)
        {
            return true;
        }

        player.RCShockWave.GetComponent<RecircleShockWaveSkill_controller>().ReturnRCShockWave();
        return false;
    }
}
