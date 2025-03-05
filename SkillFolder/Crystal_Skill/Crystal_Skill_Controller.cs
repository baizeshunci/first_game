using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    //private Animator anim => GetComponentInChildren<Animator>();
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crytalExitTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    [SerializeField] private float growSpeed;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closetTarget)
    {
        crytalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closetTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blankhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    public void Update()
    {
        crytalExitTimer -= Time.deltaTime;
        if (crytalExitTimer < 0)
        {
            FinishCrystal();
        }

        if (closestTarget != null)
        {
            if (canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

                if(Vector2.Distance(transform.position,closestTarget.position)<1f)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(8,8), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>()!=null)
            {
                PlayerManager.instance.player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equipedAmulet != null)
                {
                    equipedAmulet.Effect(hit.transform);
                }
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SetDestroy();
        }
    }

    public void SetDestroy()
    {
        Destroy(gameObject);
    }

}
