using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    CharacterBattleData[] partsArray;
    bool isPartDestroy = false;     // �����ı�

    public void Init()
    {
        // 2~4�� ������ ������ �������� ����
        int random = Random.Range(2, 5);
        partsArray = new CharacterBattleData[random];
        foreach (var part in partsArray)
        {
            part.Init();
        }
    }

    public void StartTurn()
    {
        if (isPartDestroy)
        {
            isPartDestroy = false;

            // ���� �ı� �� ���� �Ұ���
        }

        foreach (var part in partsArray)
        {
            part.StartTurn();
        }
    }

    public void EndTurn()
    {
        foreach (var part in partsArray)
        {
            part.EndTurn();
        }
    }

    public void Damaged(DamageInfo damage, int partIdx)
    {
        if (partIdx < 0 || partIdx >= partsArray.Length) return;
        partsArray[partIdx].Damaged(damage);
    }

    public List<SkillData> GetSkillData()
    {
        List<SkillData> skillDatas = new List<SkillData>(); 
        foreach (var part in partsArray)
        {
            var partSkills = part.GetSkillData();
            foreach (var skill in partSkills)
            {
                skillDatas.Add(skill);
            }
        }

        return skillDatas;
    }
}
