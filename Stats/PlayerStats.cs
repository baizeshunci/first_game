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
}
