using Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    CharacterBattleData[] partsArray;
    bool isPartDestroy = false;     // �����ı�

    // ��ü
    CharacterBattleData bodyData;

    public void Init()
    {
        bodyData = new CharacterBattleData();
        bodyData.Init();

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

    public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx)
    {
        // ���� �ܿ� ������ ��ü�� ����.
        if (partIdx < 0 || partIdx >= partsArray.Length || partsArray[partIdx] == null)
        {
            bodyData.Damaged(damage, coinSuccess);
            return;
        }

        partsArray[partIdx].Damaged(damage, coinSuccess, partIdx);
        // ���� �ı�
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

            // ���� �ı� �� �� �ϰ� ���� �Ұ���
            return null;
        }

        List<SkillData> skillDatas = new List<SkillData>();
        foreach (var part in partsArray)
        {
            // �ı��� ������ ���� �Ұ���
            if(part.IsDead) { continue; }

            skillDatas.Add(part.GetSkillData().FirstOrDefault());
        }
        // ������ ��ų�� ��ü�� ����
        skillDatas.Add(new SkillData());

        // ������ ��ų�� ����� �ϴ� ��찡 ������ ���⼭ ó��

        return skillDatas;
    }

    public bool IsDead { get { return bodyData.IsDead; } }
}
