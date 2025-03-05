using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate=35;
    [Header("Crystal instead of clone")]
    public bool crystalInseadOfClone;

    public void CreateClone(Transform _clonePosition,Vector3 _offset,int _faceTarget)
    {
        if(crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration,canAttack,_offset,_faceTarget, FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero, 1);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if(createCloneOnDashOver)
        {
            CreateClone(player.transform , Vector3.zero, 1);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, -0.7f)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _Transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_Transform,_offset, 2);
    }
}
