using Battle;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    // 공격 타입
    private EAttackType attackType;
    public EAttackType AttackType { get { return attackType; } }
    // 공격 속성
    private ESinAffinities sinAffinities;
    public ESinAffinities SinAffinities { get {  return sinAffinities; } }

    private int defaultDamage = 0;      // 기본 데미지
    public int DefaultDamage { get { return defaultDamage; } }
    private int coinCount = 0;            // 코인 갯수
    public int CoinCount { get { return coinCount; } }
    private int coinDamage = 0;         // 코인 데미지
    public int CoinDamage { get { return coinDamage; } }

    public void Init()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(Battle.EAttackType)).Length);
        attackType = (Battle.EAttackType)random;

        random = Random.Range(0, System.Enum.GetValues(typeof(Battle.ESinAffinities)).Length);
        sinAffinities = (Battle.ESinAffinities)random;

        random = Random.Range(BattleInfo.Min_DefaultDamage, BattleInfo.Max_DefaultDamage + 1);
        defaultDamage = random;
        random = Random.Range(BattleInfo.Min_CoinCount, BattleInfo.Max_CoinCount + 1);
        coinCount = random;
        random = Random.Range(BattleInfo.Min_CoinDamage, BattleInfo.Max_CoinDamage + 1);
        coinDamage = random;
    }
}
