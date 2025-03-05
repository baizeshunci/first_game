using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;
    protected float xInput;
    protected float yInput;
    protected string animBoolName;

    protected float stateTimer = -1f;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }


    public virtual void Update()
    {

        stateTimer -=Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
        if (stateTimer < 0 && !player.isBusy)
        {
            player.SetVelocity(xInput * player.movespeed, rb.velocity.y);
        }
        player.anim.SetFloat("yVelocity",rb.velocity.y);

        
    }


    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
  
    }

    public virtual void AnimatorFinishTrigger()
    {
        triggerCalled = true;
    }
    

}
