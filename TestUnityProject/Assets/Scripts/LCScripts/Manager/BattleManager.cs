using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Àû
    [SerializeField]
    EnemyBattleData enemy;

    // ¾Æ±º
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
