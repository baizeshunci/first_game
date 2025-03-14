using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public RecircleShockWave RSWSkill { get; private set; }
    public Blankhole_Skill blankhole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    public Parry_Skill parry { get; private set; }
    public Dodge_Skill dodge { get; private set; }
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        RSWSkill = GetComponent<RecircleShockWave>();
        blankhole = GetComponent<Blankhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
        parry = GetComponent<Parry_Skill>();
        dodge = GetComponent<Dodge_Skill>();
    }
}
