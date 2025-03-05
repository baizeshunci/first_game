using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{


    public bool UseDashSkill()
    {
        Debug.Log("Created clone behind");
        return base.UseSkill();
    }
}
