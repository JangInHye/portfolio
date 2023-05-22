using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투에 사용되는 스킬 클래스
/// </summary>
public class SkillBlock : MonoBehaviour
{
    SkillData[] skills;
    CharacterBattleData myChar;

    public void Init(SkillData[] inSkills, CharacterBattleData inChar)
    {
        skills = inSkills;
        myChar = inChar;    
    }

    public int GetSpeed { get { return myChar.CurSpeed; } }
}
