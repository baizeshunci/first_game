using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemes_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        if(PlayerManager.instance.player.GetComponent<PlayerStats>().currentHP >
            PlayerManager.instance.player.GetComponent<PlayerStats>().GetMaxHealthValue() * 0.1f)
        {
            return;
        }
        if (!Inventory.instance.CanUseArmor())
        {
            Debug.Log("shibai");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
