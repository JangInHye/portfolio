using Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    CharacterBattleData[] partsArray;
    bool isPartDestroy = false;     // 부위파괴

    // 본체
    CharacterBattleData bodyData;

    public void Init()
    {
        bodyData = new CharacterBattleData();
        bodyData.Init();

        // 2~4개 정도의 부위를 랜덤으로 가짐
        int random = Random.Range(2, 5);
        partsArray = new CharacterBattleData[random];
        foreach (var part in partsArray)
        {
            part.Init();
        }
    }

    public void StartTurn()
    {
        foreach (var part in partsArray)
        {
            part.StartTurn();

            // 디버프 처리
        }
    }

    public void EndTurn()
    {
        foreach (var part in partsArray)
        {
            part.EndTurn();

            // 디버프 감소 및 처리
        }
    }

    public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx)
    {
        // 부위 외에 공격은 본체로 간다.
        if (partIdx < 0 || partIdx >= partsArray.Length || partsArray[partIdx] == null)
        {
            bodyData.Damaged(damage, coinSuccess);
            return;
        }

        partsArray[partIdx].Damaged(damage, coinSuccess, partIdx);
        // 부위 파괴
        if (partsArray[partIdx].IsDead)
        {
            isPartDestroy = true;
        }
    }

    public List<SkillData> GetSkillData()
    {
        if (isPartDestroy)
        {
            isPartDestroy = false;

            // 부위 파괴 시 한 턴간 전투 불가능
            return null;
        }

        List<SkillData> skillDatas = new List<SkillData>();
        foreach (var part in partsArray)
        {
            // 파괴된 부위는 전투 불가능
            if(part.IsDead) { continue; }

            skillDatas.Add(part.GetSkillData().FirstOrDefault());
        }
        // 마지막 스킬은 본체로 고정
        skillDatas.Add(new SkillData());

        // 강제로 스킬을 띄워야 하는 경우가 있으면 여기서 처리

        return skillDatas;
    }

    public bool IsDead { get { return bodyData.IsDead; } }
}
