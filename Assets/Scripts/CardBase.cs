using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase
{
    // 卡牌基本内容 卡牌名称，防御，技能
    public string CardName;
    public int Defense;
    public Skill[] SkillList;
    public int CardID;
    public int Attack;
    public CardBase(int cardid, string cardName,int attack,int defense,Skill[] skillList = null){
        CardID = cardid;
        CardName = cardName;
        Defense =defense;
        SkillList = skillList;
        Attack = attack;
    }
}
