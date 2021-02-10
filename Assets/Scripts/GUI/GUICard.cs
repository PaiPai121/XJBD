using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICard : MonoBehaviour
{
    // card information
    public Texture BackGroundImg;
    public Texture[] Profile;
    public List<Texture> SkillIconList;
    public List<Texture> SkillPointList;
    // public int defense = 2;
    public List<Skill> skillList;

    public bool drawCard = true;


    public int cardWidth = 200;
    public float HWratio = 0.6f;
    public int Horizontal = 10;
    public int Vertical = 10;

    public float ProfileCor = 0.3f;

    public float defenseHor = 0.03f;
    public float defenseVer = 0.778f;

    public int fontSize = 10;
    public Font font;

    public int SkillStartx;
    public int SkillStarty;
    public float SkillIconA;
    public float SkillTextA;
    public float SkillTextH;

    public CardBase cardForShow;
    private int CardHeight;

    private GUIStyle defenseStyle;
    private GUIStyle skillStyle;
    private Skill skill; // 测试用
    private void Start(){
        defenseStyle = new GUIStyle ();
		defenseStyle.fontSize = fontSize;
		defenseStyle.fontStyle = FontStyle.Bold;
		defenseStyle.alignment = TextAnchor.MiddleCenter;
        defenseStyle.font = font;
        // skill = new Skill("水流击",1,1,1,1,"如子弹一般掷出\n水滴，range 2，对面前横向最多\n三个敌人challenge 2(钝器伤害)");
        skillStyle = new GUIStyle();
        skillStyle.fontSize = fontSize - 6;
        skillStyle.fontStyle = FontStyle.Bold;
        skillStyle.alignment = TextAnchor.UpperLeft;
        skillStyle.normal.textColor = new Color(255,255,255);
        skillStyle.font = font;
        // for (int i = 0; i < 3; i++){
        //     skillList.Add(skill);
        // }
    }

    private void OnGUI(){
        if (drawCard){
        // GUI更新时调用
        CardHeight = (int)(cardWidth * HWratio);
        // 绘制卡牌背景
        GUI.Label(new Rect(Horizontal,Vertical,cardWidth,CardHeight),BackGroundImg);  // 绘制卡牌底
        //绘制角色立绘
        GUI.Label(new Rect(Horizontal,(CardHeight * 0.75f - cardWidth*ProfileCor)/2 + Vertical, cardWidth*ProfileCor,cardWidth*ProfileCor),Profile[cardForShow.CardID]); // 角色立绘
        
        // 绘制Defense
        GUI.Label(new Rect(Horizontal + defenseHor * cardWidth, Vertical + defenseVer * CardHeight , cardWidth * 0.04f , cardWidth * 0.04f ),cardForShow.Defense.ToString(),defenseStyle);

        // 绘制技能
        for (int i = 0; i < cardForShow.SkillList.Length; i++){
            SkillDraw(cardForShow.SkillList[i],SkillStartx,SkillStarty + (int)(i*  SkillIconA * cardWidth*2));
        }
        
        }



    }

    private void SkillDraw(Skill skill,int startx,int starty){
        // 技能图标
        Texture skillPoint = SkillPointList[skill.SkillType];
        Texture skillIcon = SkillPointList[skill.SkillType];

        // 绘制ICON
        GUI.Label(new Rect(startx,starty, SkillIconA * cardWidth , SkillIconA*cardWidth )   ,skillIcon );
        // 绘制文本
        string nameAndDes = skill.Name+" : "+skill.Description;

        GUI.Label(new Rect(startx + SkillIconA * cardWidth *1.3f ,starty, 16 * SkillTextA , nameAndDes.Length/16*SkillTextH  )   ,nameAndDes,skillStyle);
    }


}
