using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power by 1 Á¦Á¿
    public Stat agility; // 1 point increase damage by 1% and crit.chance by 1% Ãô½Ý
    public Stat intelligence; // 1 point inrease magic damage by 1 and magic resistance by 3 ÖÇ»Û
    public Stat vitality; // 1 point increase health by 3 or 4 points »îÁ¦

    [Header("Offensive stats")]
    public Stat damage; //ÉËº¦
    public Stat critChance;  //±©»÷ÂÊ
    public Stat critDamage;
    public Stat critPower;  //±¬ÉË


    [Header("Defensive stats")]
    public Stat maxHP;  //ÂúÑª
    public Stat armor; // ×°¼× defence 
    public Stat evasion; //ÉÁ±Ü miss
    public Stat magicResistance;  //·¨¿¹

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;  // does damage over time
    public bool isChilled;  // record armor by 20%
    public bool isShocked;  // record accuracy by 20%

    [SerializeField] private float ailmentsDuration = 2;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int igniteDamage;

    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    private SpriteRenderer sr;
    [SerializeField] public int currentHP;

    public System.Action onHealthChanged;
    public bool isDead {  get; private set; }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        critPower.SetDefaultValue(150);
        currentHP = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
            isShocked = false;
        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    public virtual void IncreaseStatBy(int _modifier ,float _duration ,Stat _statToModify)
    {
        // start corototuine for stat increase
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;


        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculteCriticalDamage(totalDamage);
            //Debug.Log("Total crit damage is " + totalDamage);
        }
        totalDamage = CheckTargetTotalArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        //if invnteroy current weapon has fire effect
        //DoMagicDamage(_targetStats);

        DoMagicDamage(_targetStats);// remove if you don't want to apply magic hit on primary attack
    }

    #region Magical damage and ailments
    private void ApplyIgniteDamage()
    {
        if(ignitedDamageTimer < 0)
        {
            //Debug.Log("Take burn damage" + igniteDamage);

            DecreaseHealthy(igniteDamage);

            if (currentHP <= 0 && !isDead)
            {
                Die();
                isIgnited = false;
            }

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplayShock = _lightningDamage > _iceDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplayShock)
        {
            if (UnityEngine.Random.value < .33f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplayShock);
                Debug.Log("»ð");
                return;
            }

            if (UnityEngine.Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplayShock);
                Debug.Log("±ù");
                return;
            }

            if (_lightningDamage > 0)
            {
                canApplayShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplayShock);
                Debug.Log("µç");
                return;
            }
        }

        if (canApplyIgnite)
        {
            Debug.Log("canApplyIgnite");
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplayShock)
        {
            Debug.Log("canApplayShock");
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplayShock);
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        Debug.Log("ApplyAilments works");
        bool canApplyIgnte = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnte)
        {
            Debug.Log("ignite");
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            Debug.Log("ignite 2");
            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            Debug.Log("chill");
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            Debug.Log("shock");
            if (!isShocked)
            {
                Debug.Log("ApplyShock");
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearstTargetWithShockStrike();
            }


            //find closest target ,only among the enemies
            // instatnitate thunder strike
            // setup thunder strike
        }
    }

    public void ApplyShock(bool _shock)
    {

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearstTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

        float closetDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1f)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closetDistance)
                {
                    closetDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

        }
        if (closestEnemy == null)
            closestEnemy = transform;
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderStrike_Controll>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthy(_damage);

        Debug.Log(_damage);

        GetComponent<Entity>().DamageEX();

        if (currentHP <= 0 && !isDead)
        {
            Die();
            isIgnited = false;
        }
    }

    public virtual void IncreaseHealthy(int _amount)
    {
        currentHP += _amount;

        if (currentHP >= GetMaxHealthValue())
        {
            currentHP = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public virtual void DecreaseHealthy(int _damage)
    {
        currentHP -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
        //throw new NotImplementedException();
    }

    #region Stat calculations
    private int CheckTargetTotalArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 1, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if(isShocked)
        {
            totalEvasion  -= 20;
        }

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    private int CalculteCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        //Debug.Log("total crit power %" +  totalCritPower);
        float critDamage = _damage * totalCritPower;
        //Debug.Log("crit damage before round up" + critDamage);
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    //public void UpdateMaxHealthValue()
    //{
    //    maxHP. = GetMaxHealthValue();
    //}
}
