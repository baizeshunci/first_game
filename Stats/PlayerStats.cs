using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        PlayerManager.instance.player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    public override void DecreaseHealthy(int _damage)
    {
        base.DecreaseHealthy(_damage);

        if (Inventory.instance.GetEquipment(EquipmentType.Armor) == null)
        {
            return;
        }

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.Effect(PlayerManager.instance.player.transform);
        }
    }

    public override void onEvasion()
    {
        PlayerManager.instance.player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }

        if (CanCrit())
        {
            totalDamage = CalculteCriticalDamage(totalDamage);
            //Debug.Log("Total crit damage is " + totalDamage);
        }
        totalDamage = CheckTargetTotalArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        //if invnteroy current weapon has fire effect
        //DoMagicDamage(_targetStats);

        DoMagicDamage(_targetStats);
    }
}
