using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet, //�����
    Flask   //ҩƿ
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength; // 1 point increase damage by 1 and crit.power by 1 ����
    public int agility; // 1 point increase damage by 1% and crit.chance by 1% ����
    public int intelligence; // 1 point inrease magic damage by 1 and magic resistance by 3 �ǻ�
    public int vitality; // 1 point increase health by 3 or 4 points ����

    [Header("Offensive stats")]
    public int damage; //�˺�
    public int critChance;  //������
    public int critDamage;
    public int critPower;  //����


    [Header("Defensive stats")]
    public int maxHP;  //��Ѫ
    public int armor; // װ�� defence 
    public int evasion; //���� miss
    public int magicResistance;  //����

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("craft requirements")]
    public List<InventoryItem> craftMaterials;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHP.AddModifier(maxHP);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHP.RemoveModifier(maxHP);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }
}
