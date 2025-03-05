using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Heal effect",menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healParent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        // player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // how much to heal
        int healAmout = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healParent);

        // heal
        playerStats.IncreaseHealthy(healAmout);
    }
}
