using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill 
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDogeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDogeButton.GetComponent<Button>().onClick.AddListener(UnlockDoge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDoge()
    {
        if(unlockDogeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if(unlockMirageDodge.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if(dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(1.5f * player.facingDir, 0, 0), 2);
        }
    }
}
