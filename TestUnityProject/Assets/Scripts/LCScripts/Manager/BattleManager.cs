using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 적
    EnemyBattleData enemy;

    // 아군
    List<CharacterBattleData> sinnersList;
    int liveSinners;                // 살아있는 아군 수

    // 아군의 전투 횟수가 늘어날 수 있으니 List로 관리
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

        for (int i = 0; i < 5; i++)         // 5명 고정
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
    /// 턴 시작
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

        // 적의 공격대상 지정
        foreach (var eSkill in enemySkills)
        {
            // 해당 부위 공격만 가능하도록 남겨둔다.
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
    /// 스킬 지정
    /// </summary>
    public void SkillCommand(CharacterBattleData attacker, SkillBlock victim, SkillData skill)
    {
        if (eBattleState != EBattleState.WaitCommand) return;

        AttackInfo info = new AttackInfo();
        info.Attacker = attacker;
        info.Victim = victim.Character;
        info.AttackerSkill = skill;

        bool isDuel = false;
        // 적이 공격을 할 경우
        if (victim.Skill != null && victim.Skill.Length > 0)
        {
            var beforeAtkInfo = GetAttackInfo(victim.Character);
            if (beforeAtkInfo.Attacker != null)
            {
                // 같은 속도라면 본인에게 오는 공격만 합 가능
                if (attacker.CurSpeed == victim.Character.CurSpeed
                    && beforeAtkInfo.Victim == attacker)
                {
                    isDuel = true;
                }
                // 아군의 속도가 더 높을 경우 적의 공격과 합 진행
                else if (attacker.CurSpeed > victim.Character.CurSpeed)
                {
                    isDuel = true;
                }
            }

            if (isDuel)
            {
                // 기존에 합을 진행하던 게 있다면 제거
                // 적의 공격이라면
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
        // 합을 진행할 경우 낮은 속도 기준으로 공격이 진행된다.
        info.Speed = isDuel ? Mathf.Min(attacker.CurSpeed, victim.Character.CurSpeed)
                                               : attacker.CurSpeed;

    }

    /// <summary>
    /// 공격 시작
    /// </summary>
    public void AttackCommand()
    {
        if (eBattleState != EBattleState.WaitCommand) return;

        // 공격할 대상이 정해지지 않은 아군이 있다면 아직 공격을 시작할 수 없다.
        foreach (var skill in mySkills)
        {
            if (skill.IsAttacked == false) return;
        }

        // 공격할 대상이 다 정해진 후에 공격명령을 할 경우
        ChangeState(EBattleState.StartAttack);
    }

    /// <summary>
    /// 공격 시작
    /// </summary>
    private void StartAttack()
    {
        // 속도 순으로 처리 -> 우선순위 큐에 속도순으로 정렬해서 사용 예정

        // 속도가 높아도 상대의 공격을 막는 경우라면 합을 진행한다.
        // 서로 공격하는 경우라면 합을 진행

        // 공격이 전부 끝나면 턴 종료
        ChangeState(EBattleState.EndTurn);
    }

    /// <summary>
    /// 턴 종료 
    /// </summary>
    private void EndTurn()
    {
        enemy.EndTurn();
        foreach (var sinner in sinnersList)
        {
            sinner.EndTurn();
        }

        // 사망 체크
        if (enemy != null && enemy.IsDead)
        {
            // 승리
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
                // 패배
                ChangeState(EBattleState.Defeat);
            }
        }

        // 승리나 패배 했을 경우 전투를 종료한다.
        if (eBattleState != EBattleState.EndTurn)
        {
            // 결과 노출 후에 초기화
            return;
        }

        ChangeState(EBattleState.StartTurn);
    }

    private void EndBattle(bool isVictory)
    {
        // 결과 출력
        Debug.Log("endbattle : " + isVictory);

        // 전투 다시 시작
        ResetBattle();
    }

    /// <summary>
    /// 사망하면 뒤로
    /// 속도가 높으면 앞으로
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortSinners(CharacterBattleData a, CharacterBattleData b)
    {
        // 사망 판정 먼저
        if (a.IsDead) return 1;
        else if (b.IsDead) return -1;

        // 속도 판정
        if (a.CurSpeed > b.CurSpeed) return -1;
        else if (a.CurSpeed < b.CurSpeed) return 1;

        return 0;
    }
}
