using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人类，继承自Entity类
public class Enemy : Entity
{
    // 定义玩家所在的层
    [SerializeField] protected LayerMask whatIsPlayer;

    // 昏迷相关的信息
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    // 移动相关信息
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    // 攻击相关信息
    public float attackDistance;
    public float attackCooldown;
    public float lastTimeAttack;

    // 敌人状态机实例
    protected EnemyStateMachine stateMachine { get; private set; }
    // 最后一次播放的动画名称
    public string lastAnimBoolName {  get; private set; }

    // 初始化方法，设置状态机和默认移动速度
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    // 启动方法，用于初始化
    protected  override void Start()
    {
        base.Start();
    }

    // 每帧更新方法，更新状态机状态
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

    }

    // 分配最后一次播放的动画名称
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    // 慢化敌人移动速度和动画速度，指定时间后恢复默认速度
    public override void SlowEntityBy(float _slowPercentage, float SlowDuration)
    {
        moveSpeed = moveSpeed * (1-_slowPercentage);
        anim.speed = anim.speed * (1-_slowPercentage);

        Invoke("ReturnDefaultSpeed", SlowDuration);
    }

    // 恢复敌人默认移动速度
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    // 冻结或解冻敌人移动和动画
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    // 冻结敌人一段时间
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimeCoroutine(_duration)); 

    // 冻结时间的协程方法
    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window

    // 打开反击窗口，敌人可以被击昏
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    // 关闭反击窗口，敌人不能被击昏
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    // 检查敌人是否可以被击昏
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // 动画结束触发器
    public virtual void AnimationFinishTrigger()=>stateMachine.currentState.AnimationFinishTrigger();   

    // 检测玩家是否被发现
    public virtual RaycastHit2D IsPlayerDetedted() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 7, whatIsPlayer);

    // 在场景中绘制Gizmos，用于可视化攻击距离
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}