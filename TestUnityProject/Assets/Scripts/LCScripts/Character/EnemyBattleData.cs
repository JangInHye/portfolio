using Battle;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    private CharacterBattleData[] _partsArray;
    private bool _isPartDestroy = false;     // �����ı�

    private List<SkillBlock> _skillBlockList =  new List<SkillBlock>();

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

        // ��ų ������ �ʱ�ȭ
        foreach (var part in _partsArray)
        {
            _skillBlockList.Add(new SkillBlock(part, 1));
        }
        // ������ ��ų�� ��ü�� ����
        _skillBlockList.Add(new SkillBlock(_bodyData));
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
    public List<SkillBlock> SetSkillData()
    {
        if (_isPartDestroy)
        {
            _isPartDestroy = false;

            // ���� �ı� �� �� �ϰ� ���� �Ұ���
            return null;
        }

        foreach (var skillBlock in _skillBlockList)
        {
            // ��ü�� �Ѿ��.
            if (skillBlock.Character == _bodyData) continue;

            // �ı��� ������ ���� �Ұ���
            if (skillBlock.Character.IsDead)
            {
                // ����ִ� ��ų�������� ��� ���� �������� ��ü�Ѵ�.
                skillBlock.Skill = null;
                continue;
            }

            skillBlock.Skill = skillBlock.Character.GetSkillData(1);
        }

        // ������ ��ų�� ����� �ϴ� ��찡 ������ ���⼭ ó��

        return _skillBlockList;
    }

    public bool IsDead { get { return _bodyData.IsDead; } }
}
