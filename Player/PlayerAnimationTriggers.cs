using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var collider in colliders)
        {
            if(collider.GetComponent<Enemy>()!=null)
            {
                EnemyStats _target = collider.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    player.stats.DoDamage(_target);
                }
                // inventory get weapon call item effect
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);

                //collider.GetComponent<Enemy>().Damage();
                //collider.GetComponent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());

                //Debug.Log(player.stats.damage.GetValue());
            }
        }
    }

    private void LaunchRCShockWave()
    {
        PlayerManager.instance.player.RCShockWaveTime = 0;
        SkillManager.instance.RSWSkill.CreateRecircleShockWave();
    }
}
