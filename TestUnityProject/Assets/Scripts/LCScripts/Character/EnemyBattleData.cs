using Battle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (var part in partsArray)
        {
            part.StartTurn();

            // ����� ó��
        }
    }

    public void EndTurn()
    {
        foreach (var part in partsArray)
        {
            part.EndTurn();

            // ����� ���� �� ó��
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

            // ���� �ı� �� ���� �Ұ���
            return null;
        }

        List<SkillData> skillDatas = new List<SkillData>();
        foreach (var part in partsArray)
        {
            skillDatas.Add(part.GetSkillData().FirstOrDefault());
        }
        // ������ ��ų�� ����� �ϴ� ��찡 ������ ���⼭ ó��

        return skillDatas;
    }
}
