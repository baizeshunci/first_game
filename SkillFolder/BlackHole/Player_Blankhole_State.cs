using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Blankhole_State : PlayerState
{
    private float flytime = .4f;
    private bool skillUsed;

    private float defaultGravity;

    public Player_Blankhole_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimatorFinishTrigger()
    {
        base.AnimatorFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flytime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
        PlayerManager.instance.player.dashState.canDash = true;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
                if(player.skill.blankhole.UseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        if(player.skill.blankhole.SkillCompleted())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
