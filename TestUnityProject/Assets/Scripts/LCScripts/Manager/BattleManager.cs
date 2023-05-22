using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // ��
    [SerializeField]
    EnemyBattleData enemy;

    // �Ʊ�
    [SerializeField]
    CharacterBattleData[] sinnersArray = new CharacterBattleData[5];

    // �Ʊ��� ���� Ƚ���� �þ �� ������ List�� ����
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
    /// �� ����
    /// </summary>
    private void StartTurn()
    {
        // ��� üũ
    }

    /// <summary>
    ///�� ���� 
    /// </summary>
    private void EndTurn()
    {

    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void StartAttack()
    {
        // �ӵ� ������ ó��

    }
}
