using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 적
    [SerializeField]
    EnemyBattleData enemy;

    // 아군
    [SerializeField]
    CharacterBattleData[] sinnersArray = new CharacterBattleData[5];

    // 아군의 전투 횟수가 늘어날 수 있으니 List로 관리
    List<SkillBlock[]> mySkills = new List<SkillBlock[]>();
    SkillBlock[] enemySkills;

    private void Awake()
    {
        enemy.Init();
        foreach (var sinner in sinnersArray)
        {
            sinner.Init();
        }
    }

    /// <summary>
    /// 턴 시작
    /// </summary>
    private void StartTurn()
    {
        // 사망 체크
    }

    /// <summary>
    ///턴 종료 
    /// </summary>
    private void EndTurn()
    {

    }

    /// <summary>
    /// 공격 시작
    /// </summary>
    private void StartAttack()
    {
        // 속도 순으로 처리

    }
}
