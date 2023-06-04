using Battle;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // ��
    private EnemyBattleData _enemy;

    // �Ʊ�
    private List<CharacterBattleData> _sinnersList;
    private int _liveSinners;                // ����ִ� �Ʊ� ��

    // �Ʊ��� ���� Ƚ���� �þ �� ������ List�� ����
    private List<SkillBlock> _mySkills = new List<SkillBlock>();
    private List<SkillBlock> _enemySkills = new List<SkillBlock>();

    private EBattleState _eBattleState = EBattleState.None;

    private List<AttackInfo> _attackList;
    private AttackInfo GetAttackInfo(CharacterBattleData attacker, int attackerIndex)
    {
        int index = 0;
        foreach (var atk in _attackList)
        {
            if (index++ <= attackerIndex) break;

            if (atk.Attacker == attacker)
            {
                return atk;
            }
        }

        return null;
    }

    private void Awake()
    {
        _enemy = new EnemyBattleData();
        _sinnersList = new List<CharacterBattleData>();

        SkillDataHelper.Instance.Init();

        ResetBattle();
    }


    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    private void ResetBattle()
    {
        _enemy.Init();

        for (int i = 0; i < 5; i++)         // 5�� ����
        {
            _sinnersList[i].Init();
        }

        _liveSinners = _sinnersList.Count;

        ChangeState(EBattleState.StartTurn);
    }

    /// <summary>
    /// ���� ���� ��ü
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(EBattleState state)
    {
        _eBattleState = state;

        switch (_eBattleState)
        {
            case EBattleState.None:
                break;
            case EBattleState.StartTurn:
                StartTurn();
                break;
            case EBattleState.WaitCommand:
                break;
            case EBattleState.StartAttack:
                StartAttack();
                break;
            case EBattleState.EndTurn:
                EndTurn();
                break;
            case EBattleState.Victory:
                EndBattle(true);
                break;
            case EBattleState.Defeat:
                EndBattle(false);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �� ����
    /// </summary>
    private void StartTurn()
    {
        _enemy.StartTurn();
        foreach (var sinner in _sinnersList)
        {
            sinner.StartTurn();
            _mySkills.Add(sinner.GetSkillData());
        }

        _enemySkills = _enemy.GetSkillData();

        // ���� ���ݴ�� ����
        foreach (var eSkill in _enemySkills)
        {
            // ������ ���ϴ� ����
            if (eSkill.Skill == null) continue;

            int rand = Random.Range(0, _liveSinners);
            AddAttackInfo(eSkill, _mySkills[rand], eSkill.Character.CurSpeed);
        }

        ChangeState(EBattleState.WaitCommand);
    }
    
    /// <summary>
    /// ���� �߰�
    /// </summary>
    /// <param name="attackerSkill"></param>
    /// <param name="victimSkill"></param>
    /// <param name="speed"></param>
    /// <param name="aSkillIndex"></param>
    /// <param name="vSkillIndex"></param>
    private void AddAttackInfo(SkillBlock attackerSkill, SkillBlock victimSkill,
                                    int speed, int aSkillIndex = 0, int vSkillIndex = -1)
    {
        AttackInfo atkInfo = new AttackInfo();
        atkInfo.Attacker = attackerSkill.Character;
        atkInfo.Victim = victimSkill.Character;
        atkInfo.AttackerSkill = attackerSkill.Skill[aSkillIndex];
        atkInfo.VictimSkill = vSkillIndex == -1 ? null : victimSkill.Skill[vSkillIndex];

        atkInfo.BeforeDamage = attackerSkill.Skill[aSkillIndex].DefaultDamage;
        atkInfo.CoinCount = attackerSkill.Skill[aSkillIndex].CoinCount;
        atkInfo.Speed = speed;

        _attackList.Add(atkInfo);
    }

    /// <summary>
    /// ���� ���� ����Ʈ ����
    /// </summary>
    private void SortAttackInfoList()
    {
        int index = 0;
        // ���� ������ �� ���
        foreach (var attackInfo in _attackList)
        {
            // ���ݴ���� �������� �����Ͱ� ������ �ִٸ� �����´�.
            var beforeAtkInfo = GetAttackInfo(attackInfo.Victim, index++);

            if (beforeAtkInfo.VictimSkill != null)
            {
                int attackerSpeed = attackInfo.Attacker.CurSpeed;
                int victimSpeed = attackInfo.Victim.CurSpeed;
                // ���� �ӵ���� ���ο��� ���� ���ݸ� �� ����
                // �Ʊ��� �ӵ��� �� ���� ��� ���� ���ݰ� �� ����
                if ((attackerSpeed == victimSpeed && beforeAtkInfo.Victim == attackInfo.Attacker)
                    || attackerSpeed > victimSpeed)
                {
                    // ������ �ִ� ���� ����� �ٲ۴�.
                    // ���� ��� ������α� ������ ���߿� ����� ������ ������ ����
                    beforeAtkInfo.Victim = attackInfo.Attacker;

                    attackInfo.Speed = victimSpeed;
                }
            }
        }
    }

    /// <summary>
    /// ��ų ����
    /// </summary>
    public void SkillCommand(SkillBlock attacker, SkillBlock victim, SkillData skill)
    {
        if (_eBattleState != EBattleState.WaitCommand) return;

        // �ϴ� ���� ��� �� ������ �Ѵ�.
        AddAttackInfo(attacker, victim, attacker.Character.CurSpeed, skill.Index);
        SortAttackInfoList();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void AttackCommand()
    {
        if (_eBattleState != EBattleState.WaitCommand) return;

        // ������ ����� �������� ���� �Ʊ��� �ִٸ� ���� ������ ������ �� ����.
        foreach (var skill in _mySkills)
        {
            if (skill.IsAttacked == false) return;
        }

        // ������ ����� �� ������ �Ŀ� ���ݸ���� �� ���
        ChangeState(EBattleState.StartAttack);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void StartAttack()
    {
        // �ӵ� ������ ó�� -> �켱���� ť�� �ӵ������� �����ؼ� ��� ����

        // �ӵ��� ���Ƶ� ����� ������ ���� ����� ���� �����Ѵ�.
        // ���� �����ϴ� ����� ���� ����

        // ������ ���� ������ �� ����
        ChangeState(EBattleState.EndTurn);
    }

    /// <summary>
    /// �� ���� 
    /// </summary>
    private void EndTurn()
    {
        _enemy.EndTurn();
        foreach (var sinner in _sinnersList)
        {
            sinner.EndTurn();
        }

        // ��� üũ
        if (_enemy != null && _enemy.IsDead)
        {
            // �¸�
            ChangeState(EBattleState.Victory);
        }
        else if (_sinnersList != null)
        {
            // ���� �� ����ִ� ĳ���͸� ��� üũ
            _sinnersList.Sort(SortSinners);
            for (int i = _liveSinners - 1; i > -1; i--)
            {
                if (_sinnersList[i].IsDead) { _liveSinners--; }
            }

            if (_liveSinners <= 0)
            {
                // �й�
                ChangeState(EBattleState.Defeat);
            }
        }

        // �¸��� �й� ���� ��� ������ �����Ѵ�.
        if (_eBattleState != EBattleState.EndTurn)
        {
            // ��� ���� �Ŀ� �ʱ�ȭ
            return;
        }

        ChangeState(EBattleState.StartTurn);
    }

    private void EndBattle(bool isVictory)
    {
        // ��� ���
        Debug.Log("endbattle : " + isVictory);

        // ���� �ٽ� ����
        ResetBattle();
    }

    /// <summary>
    /// ����ϸ� �ڷ�
    /// �ӵ��� ������ ������
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortSinners(CharacterBattleData a, CharacterBattleData b)
    {
        // ��� ���� ����
        if (a.IsDead) return 1;
        else if (b.IsDead) return -1;

        // �ӵ� ����
        if (a.CurSpeed > b.CurSpeed) return -1;
        else if (a.CurSpeed < b.CurSpeed) return 1;

        return 0;
    }
}
