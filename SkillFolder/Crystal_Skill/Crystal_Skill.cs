using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    private Vector3 recordedPosition;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfstacks;
    [SerializeField] private float multiStackCooldown = 4;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
    protected float cooldownTimerCrystal;
    float recordBegin = -999999f;

    public override bool CanUseSkill()
    {
        bool Iscan = base.CanUseSkill();
        if(!Iscan)
        {
            return false;
        }

        if (CanUseMultiCrystal())
        {
            return true;
        }


        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return true;
            }

            Vector2 playerPos = PlayerManager.instance.player.crystalPosition.position;
            player.transform.position = recordedPosition;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, new Vector3(0, -0.7f), 2);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();

            }

        }
        return true;
    }

    public void CreateCrystal()
    {
        recordedPosition = player.transform.position;
        currentCrystal = Instantiate(crystalPrefab, PlayerManager.instance.player.crystalPosition.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    protected override void Update()
    {
        base.Update();
        cooldownTimerCrystal-= Time.deltaTime;
    }

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            //respawn crystal
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfstacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn,PlayerManager.instance.player.crystalPosition.position,Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0)
                {
                    //cooldown the skill
                    //refill crystals
                    cooldown = multiStackCooldown;
                    RefilCrystal(0);
                }
            return true;
            }
        }
        return false;
    }

    private void RefilCrystal(int add)
    {
        int amountToAdd = amountOfstacks - crystalLeft.Count + add;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        if (cooldownTimerCrystal > 0)
            return;
        recordBegin = cooldownTimerCrystal;

        cooldownTimer = multiStackCooldown;
        RefilCrystal(0);
    }
}
