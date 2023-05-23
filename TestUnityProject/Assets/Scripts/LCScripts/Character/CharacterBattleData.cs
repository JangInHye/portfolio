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

    public int CurSpeed {  get { return curSpeed; }  }

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
        if(isStagger == false)
        {
            curStaggerCount = 0;
        }
    }

    public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx = 0)
    {
        // 속성 내성 적용
        curHP -= characterData.GetDamage(damage, staggerIdx, coinSuccess);

        // 디버프나 추가 데미지가 있다면 여기서 적용

        if(curHP < characterData.staggerThreshold[staggerIdx])
        {
            curStaggerCount++;
            staggerIdx++;
            isStagger = true;
        }
    }

    public List<SkillData> GetSkillData()
    {
        // 강제로 스킬을 띄워야 하는 경우가 있으면 여기서 처리
        return characterData.GetSkillData();
    }

    public bool IsDead { get { return curHP <= 0; } }
    public bool IsPanic { get { return curMentality <= BattleInfo.Min_Mentality; } }
}
