using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // ��
    EnemyBattleData enemy;

    // �Ʊ�
    CharacterBattleData[] sinnersArray;

    // �Ʊ��� ���� Ƚ���� �þ �� ������ List�� ����
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
    /// �� ����
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
    /// ���� ��� ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_WaitCommand()
    {
        if (eBattleState != EBattleState.WaitCommand) { yield break; }
        // ������ ����� �� �������� ���ݸ������ �Ѿ��

        yield return null;

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
        foreach (var sinner in sinnersArray)
        {
            sinner.EndTurn();
        }

        // ��� üũ
        if (enemy != null && enemy.IsDead)
        {
            // �¸�
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
}
