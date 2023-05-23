using Battle;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    // 타입 내성
    public float[] attackTypeTolerance = new float[BattleInfo.Num_Type];
    // 속성 내성
    public float[] sinAffinitiesTolerance = new float[BattleInfo.Num_Sin];
    // 스킬 데이터
    SkillData[] mySkills;
    public int SkillCount { get { return mySkills == null ? 0 : mySkills.Length; } }
    public SkillData GetSkill(int idx)
    {
        if (mySkills == null || idx < 0 || mySkills.Length < idx) return new SkillData();

        return mySkills[idx];
    }

    public int mySpeed = 0;        // 속도
    public int myHP = 0;            // 체력
    public int myMentality = 0;    // 정신력

    public int[] staggerThreshold = new int[BattleInfo.Num_Stagger];     // 흐트러짐 기준

    // 원래라면 캐릭터 데이터에 따라 기본 값을 가져와야 하지만 여기에서는 전부 랜덤으로 세팅
    public void Init()
    {
        int random = 0;
        // 랜덤으로 0.7~1.5f 사이가 되도록 한다.
        for (int i = 0; i < sinAffinitiesTolerance.Length; i++)
        {
            random = Random.Range(7, 16);
            sinAffinitiesTolerance[i] = random * 0.1f;
        }

        // 2, 3개 랜덤
        random = Random.Range(2, 4);
        mySkills = new SkillData[random];
        foreach (var skill in mySkills)
        {
            skill.Init();
        }

        random = Random.Range(BattleInfo.Min_Speed, BattleInfo.Max_Speed + 1);
        mySpeed = random;
        random = Random.Range(BattleInfo.Min_HP, BattleInfo.Max_HP + 1);
        myHP = random;
        myMentality = 0;

        // 최대 체력 기준으로 설정
        SetStagger(myHP);
    }

    public void SetStagger(int hp)
    {
        for (int i = 0; i < staggerThreshold.Length; i++)
        {
            staggerThreshold[i] = (int)(myHP * BattleInfo.StaggerHP[i]);
        }
    }

    /// <summary>
    /// 데미지 계산
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <param name="stagger"></param>
    /// <returns></returns>
    public int GetDamage(AttackInfo damageInfo, int stagger, bool coinSuccess)
    {
        int result = BattleInfo.Damage(damageInfo.AttackerSkill, damageInfo.BeforeDamage, coinSuccess);

        result = (int)(result                                                   // 데미지
            * attackTypeTolerance[(int)damageInfo.AttackerSkill.AttackType]        // 공격 타입 내성
            * sinAffinitiesTolerance[(int)damageInfo.AttackerSkill.SinAffinities]      // 공격 속성 내성
            * BattleInfo.StaggerAttackRatio[stagger]);                    // 흐트러짐 여부

        return result;
    }
}
