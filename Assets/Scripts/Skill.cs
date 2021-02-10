using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string Name;
    public int Range;
    public int Damage;
    public int Cooldown;
    public int SkillType;
    public string Description;
    // 技能基本内容
    public Skill(string name,int range,int damage,int cooldown,int skillType,string description){
        Name = name;
        Range = range;
        Damage = damage;
        Cooldown = cooldown;
        Description =description;
        SkillType = skillType;
    }
}
