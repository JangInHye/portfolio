using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // ��
    EnemyBattleData enemy;

    // �Ʊ�
    List<CharacterBattleData> sinnersList;
    int liveSinners;                // ����ִ� �Ʊ� ��

    // �Ʊ��� ���� Ƚ���� �þ �� ������ List�� ����
    List<SkillBlock> mySkills = new List<SkillBlock>();
    List<SkillBlock> enemySkills = new List<SkillBlock>();

    List<AttackInfo> attackList;
    private AttackInfo GetAttackInfo(CharacterBattleData attacker)
    {
        foreach (var atk in attackList)
        {
            if (atk.Attacker == attacker)
            {
                return atk;
            }
        }
        return new AttackInfo();
    }

    EBattleState eBattleState = EBattleState.None;

    private void Awake()
    {
        enemy = new EnemyBattleData();
        sinnersList = new List<CharacterBattleData>();

        ResetBattle();
    }

    private void ResetBattle()
    {
        enemy.Init();

        for (int i = 0; i < 5; i++)         // 5�� ����
        {
            sinnersList[i].Init();
        }

        liveSinners = sinnersList.Count;

        ChangeState(EBattleState.StartTurn);
    }

    private void ChangeState(EBattleState state)
    {
        eBattleState = state;

        switch (eBattleState)
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
        enemy.StartTurn();
        foreach (var sinner in sinnersList)
        {
            sinner.StartTurn();
            mySkills.Add(sinner.GetSkillData());
        }

        enemySkills = enemy.GetSkillData();

        // ���� ���ݴ�� ����
        foreach (var eSkill in enemySkills)
        {
            // �ش� ���� ���ݸ� �����ϵ��� ���ܵд�.
            if (eSkill.Skill == null) continue;

            int rand = Random.Range(0, liveSinners);
            AddAttackInfo(eSkill.Character, sinnersList[rand], eSkill.Skill[0], eSkill.Character.CurSpeed);
        }

        ChangeState(EBattleState.WaitCommand);
    }

    private void AddAttackInfo(CharacterBattleData attacker, CharacterBattleData victim, 
        SkillData attackerSkill, int speed, SkillData victimSkill = null)
    {
        AttackInfo atkInfo = new AttackInfo();
        atkInfo.Attacker = attacker;
        atkInfo.Victim = victim;
        atkInfo.AttackerSkill = attackerSkill;
        atkInfo.VictimSkill = victimSkill;

        atkInfo.BeforeDamage = attackerSkill.DefaultDamage;
        atkInfo.CoinCount = attackerSkill.CoinCount;
        atkInfo.Speed = speed;

        attackList.Add(atkInfo);
    }

    /// <summary>
    /// ��ų ����
    /// </summary>
    public void SkillCommand(CharacterBattleData attacker, SkillBlock victim, SkillData skill)
    {
        if (eBattleState != EBattleState.WaitCommand) return;

        AttackInfo info = new AttackInfo();
        info.Attacker = attacker;
        info.Victim = victim.Character;
        info.AttackerSkill = skill;

        bool isDuel = false;
        // ���� ������ �� ���
        if (victim.Skill != null && victim.Skill.Length > 0)
        {
            var beforeAtkInfo = GetAttackInfo(victim.Character);
            if (beforeAtkInfo.Attacker != null)
            {
                // ���� �ӵ���� ���ο��� ���� ���ݸ� �� ����
                if (attacker.CurSpeed == victim.Character.CurSpeed
                    && beforeAtkInfo.Victim == attacker)
                {
                    isDuel = true;
                }
                // �Ʊ��� �ӵ��� �� ���� ��� ���� ���ݰ� �� ����
                else if (attacker.CurSpeed > victim.Character.CurSpeed)
                {
                    isDuel = true;
                }
            }

            if (isDuel)
            {
                // ������ ���� �����ϴ� �� �ִٸ� ����
                // ���� �����̶��
                if (beforeAtkInfo.Attacker == victim.Character)
                {
                    attackList.Remove(beforeAtkInfo);
                }
                // 
                else
                {

                }

                info.VictimSkill = victim.Skill[0];
            }
        }
        // ���� ������ ��� ���� �ӵ� �������� ������ ����ȴ�.
        info.Speed = isDuel ? Mathf.Min(attacker.CurSpeed, victim.Character.CurSpeed)
                                               : attacker.CurSpeed;

    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void AttackCommand()
    {
        if (eBattleState != EBattleState.WaitCommand) return;

        // ������ ����� �������� ���� �Ʊ��� �ִٸ� ���� ������ ������ �� ����.
        foreach (var skill in mySkills)
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
        enemy.EndTurn();
        foreach (var sinner in sinnersList)
        {
            sinner.EndTurn();
        }

        // ��� üũ
        if (enemy != null && enemy.IsDead)
        {
            // �¸�
            ChangeState(EBattleState.Victory);
        }
        else if (sinnersList != null)
        {
            sinnersList.Sort(SortSinners);
            for (int i = liveSinners - 1; i > -1; i--)
            {
                if (sinnersList[i].IsDead) { liveSinners--; }
            }

            if (liveSinners <= 0)
            {
                // �й�
                ChangeState(EBattleState.Defeat);
            }
        }

        // �¸��� �й� ���� ��� ������ �����Ѵ�.
        if (eBattleState != EBattleState.EndTurn)
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
