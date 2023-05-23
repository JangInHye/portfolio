using Battle;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleData : MonoBehaviour, IBattleFunction
{
    CharacterData characterData;

    int curHP = 0;
    int curMentality = 0;
    int curSpeed = 0;

    int staggerIdx = 0;             // 현재 흐트러짐 기준
    int curStaggerCount = 0;     // 흐트러진 상태라면 그 정도
    bool isStagger = false;        // 이번 턴에 흐트러진 것인지 체크

    public int CurSpeed { get { return curSpeed; } }

    public void Init()
    {
        characterData = new CharacterData();
        characterData.Init();

        curStaggerCount = 0;
    }

    // 이번 턴 세팅
    public void StartTurn()
    {
        // 속도 관련 버프나 디버프 있다면 여기서 처리
        int rand = Random.Range(-1, 2);
        curSpeed = characterData.mySpeed + rand;        // 속도 랜덤

        isStagger = false;
    }

    public void EndTurn()
    {
        // 이번 턴에 흐트러진 것이 아니라면 해제
        if (isStagger == false)
        {
            curStaggerCount = 0;
        }
    }

    public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx = 0)
    {
        // 속성 내성 적용
        curHP -= characterData.GetDamage(damage, staggerIdx, coinSuccess);

        // 디버프나 추가 데미지가 있다면 여기서 적용

        if (curHP < characterData.staggerThreshold[staggerIdx])
        {
            curStaggerCount++;
            staggerIdx++;
            isStagger = true;
        }
    }

    /// <summary>
    /// 현재 턴에 사용할 스킬 묶음
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public SkillBlock GetSkillData(int count = 2)
    {
        SkillBlock result = new SkillBlock();
        result.Skill = new SkillData[count];
        result.Character = this;

        // 패닉일 경우 공격 불가
        // 비어있는 스킬데이터의 예외처리 필요
        if (IsPanic) return result;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, characterData.SkillCount);
            result.Skill[i] = characterData.GetSkill(rand);
        }

        return result;
    }

    /// <summary>
    /// 사망 체크
    /// </summary>
    public bool IsDead { get { return curHP <= 0; } }
    /// <summary>
    /// 패닉 체크
    /// </summary>
    public bool IsPanic { get { return curMentality <= BattleInfo.Min_Mentality; } }
}
