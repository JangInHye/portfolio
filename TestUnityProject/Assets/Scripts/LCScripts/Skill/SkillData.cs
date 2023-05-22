using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    // ���� Ÿ��
    AttackType attackType;
    // ���� �Ӽ�
    SinAffinities sinAffinities;

    int defaultDamage = 0;      // �⺻ ������
    int coinCount = 0;            // ���� ����
    int coinDamage = 0;         // ���� ������

    public void Init()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(Battle.AttackType)).Length);
        attackType = (Battle.AttackType)random;

        random = Random.Range(0, System.Enum.GetValues(typeof(Battle.SinAffinities)).Length);
        sinAffinities = (Battle.SinAffinities)random;

        random = Random.Range(BattleInfo.Min_DefaultDamage, BattleInfo.Max_DefaultDamage + 1);
        defaultDamage = random;
        random = Random.Range(BattleInfo.Min_CoinCount, BattleInfo.Max_CoinCount + 1);
        coinCount = random;
        random = Random.Range(BattleInfo.Min_CoinDamage, BattleInfo.Max_CoinDamage + 1);
        coinDamage = random;
    }

    public DamageInfo GetAttackDamage(int coin)
    {
        DamageInfo result = new DamageInfo();

        result.AttackType = attackType;
        result.SinAffinities = sinAffinities;

        int cDamage = 0;
        // ������ ������ ���� �������� �������� ���
        if (coinDamage < 0) 
            cDamage = Mathf.Abs(coinDamage * (coinCount - coin));
        else
            cDamage = coinDamage * coin;

        // ������ ���� ������ �ִٸ� ���⼭ ó��
        result.Damage = defaultDamage + cDamage;

        return result;
    }
}
