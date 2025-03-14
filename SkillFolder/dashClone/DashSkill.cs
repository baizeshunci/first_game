using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked{get; private set ;}

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked{get; private set ;}

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;
    public bool cloneOnArrivalUnlocked {get; private set ;}

    public override bool UseSkill()
    {
        return base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(unlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(unlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(unlockCloneOnArrival);
    }

    private void unlockDash()
    {
        Debug.Log("Attemtpt to unlock dash");

        if(dashUnlockButton.unlocked)
        {
            Debug.Log("Dash unlocked");
            dashUnlocked = true;
        }
    }

    private void unlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void unlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockedButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }

        public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero, 1);
        }
    }

    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform , Vector3.zero, 1);
        }
    }
}
