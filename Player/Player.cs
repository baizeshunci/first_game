using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class Player : Entity
{
    [SerializeField] protected int facingDir1;
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDurtion = .2f;
    
    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float movespeed = 5f;
    public float jumpspeed = 8f;
    public float RCShockWaveReturnImpact;
    public float defaultJumpForce;
    public float defaultMoveSpeed;

    [Header("Dash info")]
    public float dashSpeed = 15f;
    public float dashDuration=0.4f;
    public float dashDir { get; private set; }
    public float defaultDashSpeed;

    public SkillManager skill {  get; private set; }
    public GameObject RCShockWave{ get; private set; }
    public int RCShockWaveTime = 0 ;
    public Transform recircleShockWavePosition;
    public Transform crystalPosition;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }

    public PlayerFallState fallState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }

    public PlayerWallJumpState playerWallJump { get; private set; }

    public PlayerPrimaryAttack primaryAttack { get; private set; }

    public PlayerCounterAttackState counterAttack { get; private set; }

    public PlayerAimShockWaveState aimShockWave { get; private set; }

    public PlayerCatchShockWavesState catchShockWavesSttate { get; private set; }

    public PlayerGroundState groundState { get; private set; }

    public Player_Blankhole_State blankholeState { get; private set; }

    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");

        moveState = new PlayerMoveState(this, stateMachine, "Move");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");

        fallState = new PlayerFallState(this, stateMachine, "Jump");

        dashState = new PlayerDashState(this, stateMachine, "Dash");

        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");

        playerWallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");

        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimShockWave = new PlayerAimShockWaveState(this, stateMachine, "AimShockWave");

        catchShockWavesSttate = new PlayerCatchShockWavesState(this, stateMachine, "CatchShockWave");

        groundState = new PlayerGroundState(this, stateMachine, "Ground");

        blankholeState = new Player_Blankhole_State(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = movespeed;
        defaultJumpForce = jumpspeed;
        defaultDashSpeed = dashSpeed;
    }
    
    protected override void Update()
    {
        base.Update();
        facingDir1=facingDir;
        stateMachine.currentState.Update();

        CheckForDashInput();
        
        if(Input.GetKeyDown(KeyCode.T) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.UseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        movespeed = defaultMoveSpeed;
        jumpspeed = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public override void SlowEntityBy(float _slowPercentage, float SlowDuration)
    {
        movespeed = movespeed * (1- _slowPercentage);
        jumpspeed = jumpspeed * (1- _slowPercentage);
        dashSpeed = dashSpeed * (1- _slowPercentage);
        anim.speed = anim.speed * (1- _slowPercentage);

        Invoke("ReturnDefaultSpeed", SlowDuration);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        

        yield return new WaitForSeconds(_seconds);

        
        isBusy = false;
    }

    public void AssignNewRCShockWave(GameObject _newRCSW)
    {
        RCShockWave = _newRCSW;
    }

    public void CatchTheRCShockWave()
    {
        stateMachine.ChangeState(catchShockWavesSttate);
        Destroy(RCShockWave);
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimatorFinishTrigger();

    private void CheckForDashInput()
    {
        if(IsWallDectected())
        {
            return;
        }

        if(skill.dash.dashUnlocked == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.UseSkill() && dashState.canDash)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(PlayerManager.instance.player.deadState);
    }
}
