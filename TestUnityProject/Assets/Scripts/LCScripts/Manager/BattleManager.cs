using Battle;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 적
    private EnemyBattleData _enemy;

    // 아군
    private List<CharacterBattleData> _sinnersList;
    private int _liveSinners;                // 살아있는 아군 수

    // 아군의 전투 횟수가 늘어날 수 있으니 List로 관리
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
    /// 초기화
    /// </summary>
    private void ResetBattle()
    {
        _enemy.Init();

        for (int i = 0; i < 5; i++)         // 5명 고정
        {
            _sinnersList[i].Init();
        }

        _liveSinners = _sinnersList.Count;

        ChangeState(EBattleState.StartTurn);
    }

    /// <summary>
    /// 전투 상태 교체
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
    /// 턴 시작
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

        // 적의 공격대상 지정
        foreach (var eSkill in _enemySkills)
        {
            // 공격을 못하는 상태
            if (eSkill.Skill == null) continue;

            int rand = Random.Range(0, _liveSinners);
            AddAttackInfo(eSkill, _mySkills[rand], eSkill.Character.CurSpeed);
        }

        ChangeState(EBattleState.WaitCommand);
    }
    
    /// <summary>
    /// 공격 추가
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
    /// 공격 정보 리스트 정리
    /// </summary>
    private void SortAttackInfoList()
    {
        int index = 0;
        // 적이 공격을 할 경우
        foreach (var attackInfo in _attackList)
        {
            // 공격대상이 공격중인 데이터가 기존에 있다면 가져온다.
            var beforeAtkInfo = GetAttackInfo(attackInfo.Victim, index++);

            if (beforeAtkInfo.VictimSkill != null)
            {
                int attackerSpeed = attackInfo.Attacker.CurSpeed;
                int victimSpeed = attackInfo.Victim.CurSpeed;
                // 같은 속도라면 본인에게 오는 공격만 합 가능
                // 아군의 속도가 더 높을 경우 적의 공격과 합 진행
                if ((attackerSpeed == victimSpeed && beforeAtkInfo.Victim == attackInfo.Attacker)
                    || attackerSpeed > victimSpeed)
                {
                    // 기존에 있던 공격 대상을 바꾼다.
                    // 공격 명령 순서대로기 때문에 나중에 명령한 공격이 덮어씌우는 형식
                    beforeAtkInfo.Victim = attackInfo.Attacker;

                    attackInfo.Speed = victimSpeed;
                }
            }
        }
    }

    /// <summary>
    /// 스킬 지정
    /// </summary>
    public void SkillCommand(SkillBlock attacker, SkillBlock victim, SkillData skill)
    {
        if (_eBattleState != EBattleState.WaitCommand) return;

        // 일단 공격 등록 후 정리를 한다.
        AddAttackInfo(attacker, victim, attacker.Character.CurSpeed, skill.Index);
        SortAttackInfoList();
    }

    /// <summary>
    /// 공격 시작
    /// </summary>
    public void AttackCommand()
    {
        if (_eBattleState != EBattleState.WaitCommand) return;

        // 공격할 대상이 정해지지 않은 아군이 있다면 아직 공격을 시작할 수 없다.
        foreach (var skill in _mySkills)
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
        _enemy.EndTurn();
        foreach (var sinner in _sinnersList)
        {
            sinner.EndTurn();
        }

        // 사망 체크
        if (_enemy != null && _enemy.IsDead)
        {
            // 승리
            ChangeState(EBattleState.Victory);
        }
        else if (_sinnersList != null)
        {
            // 정렬 후 살아있는 캐릭터만 사망 체크
            _sinnersList.Sort(SortSinners);
            for (int i = _liveSinners - 1; i > -1; i--)
            {
                if (_sinnersList[i].IsDead) { _liveSinners--; }
            }

            if (_liveSinners <= 0)
            {
                // 패배
                ChangeState(EBattleState.Defeat);
            }
        }

        // 승리나 패배 했을 경우 전투를 종료한다.
        if (_eBattleState != EBattleState.EndTurn)
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
