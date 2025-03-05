using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if(sr.color.a <0)
            {
                Destroy(gameObject);
            }
        }
    }

    //public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset,Transform _closestEnemy)
    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,int _FaceTarget, Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        chanceToDuplicate = _chanceToDuplicate;
        canDuplicateClone = _canDuplicate;

        if (_FaceTarget == 1)
        {
            FaceDirTarget();
        }
        else if (_FaceTarget == 2)
        {
            FaceClosestTarget();
        }
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.stats.DoDamage(collider.GetComponent<CharacterStats>());
                if(canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(collider.transform, new Vector3(1f * facingDir, -0.7f), 2);
                    }
                }
            }
        }
    }
    #region    Filp
    //根据玩家的朝向方向进行翻转
    private void FaceDirTarget()
    {
        if( PlayerManager.instance.player.dashDir < 0 )
        {
            transform.Rotate(0, 180, 0);
        }
    }

    //根据敌人的方位进行翻转
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }

    }
    #endregion
}
