using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Skeleton skeleton =>GetComponentInParent<Skeleton>();
    private void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position, skeleton.attackCheckRadius);

        foreach(var collider2 in colliders)
        {
            if (collider2.GetComponent<Player>() != null)
            {
                PlayerStats target = collider2.GetComponent<PlayerStats>();
                skeleton.stats.DoDamage(target);
                //collider2.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterWindow() => skeleton.OpenCounterAttackWindow();
    private void CloseCounterWindow() => skeleton.CloseCounterAttackWindow();
}
