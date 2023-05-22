using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���Ǵ� ��ų Ŭ����
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
