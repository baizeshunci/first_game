using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f,1f)]
    [SerializeField] private float percantsgrModifier=.4f;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        
        Modify(maxHP);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i <= level; i++)
        {
            float modifer = _stat.GetValue() * percantsgrModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifer));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
