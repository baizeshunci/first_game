using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMUltiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggrestive clone")]
    [SerializeField] private UI_SkillTreeSlot aggrestiveCloneUnlockButton;
    [SerializeField] private float aggrestiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect{get;private set;}

    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInseadUnlockButton;
    public bool crystalInseadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggrestiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockedMultiClone);
        crystalInseadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }


    #region Unlock region
    private void UnlockCloneAttack()
    {
        if(cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMUltiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if(aggrestiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMUltiplier = aggrestiveCloneAttackMultiplier;
        }
    } 

    private void UnlockedMultiClone()
    {
        if(multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMUltiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if(crystalInseadUnlockButton.unlocked)
        {
            crystalInseadOfClone = true;
        }
    }

    #endregion

    public void CreateClone(Transform _clonePosition,Vector3 _offset,int _faceTarget)
    {
        if(crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration,canAttack,_offset,_faceTarget, FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,1);
    }



    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, 0, 0)));
    }

    public IEnumerator CreateCloneWithDelay(Transform _Transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_Transform,_offset, 2);
    }
}
