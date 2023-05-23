using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 적
    EnemyBattleData enemy;

    // 아군
    CharacterBattleData[] sinnersArray;

    // 아군의 전투 횟수가 늘어날 수 있으니 List로 관리
    List<SkillBlock[]> mySkills = new List<SkillBlock[]>();
    SkillBlock[] enemySkills;

    EBattleState eBattleState = EBattleState.None;

    private void Awake()
    {
        enemy = new EnemyBattleData();
        sinnersArray = new CharacterBattleData[5];

        ResetBattle();
    }

    private void ResetBattle()
    {
        enemy.Init();
        foreach (var sinner in sinnersArray)
        {
            sinner.Init();
        }

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
                StartCoroutine(C_WaitCommand());
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
        foreach (var sinner in sinnersArray)
        {
            sinner.StartTurn();
        }

        ChangeState(EBattleState.WaitCommand);
    }

    /// <summary>
    /// 공격 명령 대기
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_WaitCommand()
    {
        if (eBattleState != EBattleState.WaitCommand) { yield break; }
        // 공격할 대상이 다 정해지면 공격명령으로 넘어가기

        yield return null;

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
        foreach (var sinner in sinnersArray)
        {
            sinner.EndTurn();
        }

        // 사망 체크
        if (enemy != null && enemy.IsDead)
        {
            // 승리
            ChangeState(EBattleState.Victory);
        }
        else if (sinnersArray != null)
        {
            bool isAllDead = true;
            foreach (var sinner in sinnersArray)
            {
                if (sinner.IsDead == false)
                {
                    isAllDead = false;
                    break;
                }
            }
            if (isAllDead)
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
}
