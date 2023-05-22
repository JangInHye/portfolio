using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    // 공격 타입
    AttackType attackType;
    // 공격 속성
    SinAffinities sinAffinities;

    int defaultDamage = 0;      // 기본 데미지
    int coinCount = 0;            // 코인 갯수
    int coinDamage = 0;         // 코인 데미지

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
        // 코인이 실패할 수록 데미지가 강해지는 경우
        if (coinDamage < 0) 
            cDamage = Mathf.Abs(coinDamage * (coinCount - coin));
        else
            cDamage = coinDamage * coin;

        // 데미지 증가 버프가 있다면 여기서 처리
        result.Damage = defaultDamage + cDamage;

        return result;
    }
}
