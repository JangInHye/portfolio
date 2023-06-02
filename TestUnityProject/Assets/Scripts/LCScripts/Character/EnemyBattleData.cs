using Battle;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    private CharacterBattleData[] _partsArray;
    private bool _isPartDestroy = false;     // �����ı�

    // ��ü
    private CharacterBattleData _bodyData;

    public void Init()
    {
        _bodyData = new CharacterBattleData();
        _bodyData.Init();

        // 2~4�� ������ ������ �������� ����
        int random = Random.Range(2, 5);
        _partsArray = new CharacterBattleData[random];
        foreach (var part in _partsArray)
        {
            part.Init();
        }
    }

    public void StartTurn()
    {
        foreach (var part in _partsArray)
        {
            part.StartTurn();

            // ����� ó��
        }
    }

    public void EndTurn()
    {
        foreach (var part in _partsArray)
        {
            part.EndTurn();

            // ����� ���� �� ó��
        }
    }

    public void Damaged(AttackInfo damage, int partIdx)
    {
        // ���� �ܿ� ������ ��ü�� ����.
        if (partIdx < 0 || partIdx >= _partsArray.Length || _partsArray[partIdx] == null)
        {
            _bodyData.Damaged(damage);
            return;
        }

        _partsArray[partIdx].Damaged(damage, partIdx);
        // ���� �ı�
        if (_partsArray[partIdx].IsDead)
        {
            _isPartDestroy = true;
        }
    }

    /// <summary>
    /// �� �������� 1���� ��ų + ��ü 1�� ����
    /// </summary>
    /// <returns></returns>
    public List<SkillBlock> GetSkillData()
    {
        if (_isPartDestroy)
        {
            _isPartDestroy = false;

            // ���� �ı� �� �� �ϰ� ���� �Ұ���
            return null;
        }

        List<SkillBlock> skillDatas = new List<SkillBlock>();
        SkillBlock sb;
        foreach (var part in _partsArray)
        {
            // �ı��� ������ ���� �Ұ���
            if (part.IsDead)
            {
                // ����ִ� ��ų�������� ��� ���� �������� ��ü�Ѵ�.
                sb = new SkillBlock();
                sb.Character = part;
                skillDatas.Add(sb);
                continue;
            }

            skillDatas.Add(part.GetSkillData());
        }
        // ������ ��ų�� ��ü�� ����
        sb = new SkillBlock();
        sb.Character = _bodyData;
        skillDatas.Add(sb);

        // ������ ��ų�� ����� �ϴ� ��찡 ������ ���⼭ ó��

        return skillDatas;
    }

    public bool IsDead { get { return _bodyData.IsDead; } }
}
