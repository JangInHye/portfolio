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

    private void Awake()
    {
        enemy.Init();
        foreach (var sinner in sinnersArray)
        {
            sinner.Init();
        }
    }
}
