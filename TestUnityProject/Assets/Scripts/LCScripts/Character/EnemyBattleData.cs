using Battle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    CharacterBattleData[] partsArray;
    bool isPartDestroy = false;     // 부위파괴

    public void Init()
    {
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

    public void Damaged(DamageInfo damage, int partIdx)
    {
        if (partIdx < 0 || partIdx >= partsArray.Length) return;
        partsArray[partIdx].Damaged(damage);
    }

    public List<SkillData> GetSkillData()
    {
        if (isPartDestroy)
        {
            isPartDestroy = false;

            // 부위 파괴 시 전투 불가능
            return null;
        }

        List<SkillData> skillDatas = new List<SkillData>();
        foreach (var part in partsArray)
        {
            skillDatas.Add(part.GetSkillData().FirstOrDefault());
        }
        // 강제로 스킬을 띄워야 하는 경우가 있으면 여기서 처리

        return skillDatas;
    }
}
