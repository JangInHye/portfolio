using Battle;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    // ���� Ÿ��
    private EAttackType attackType;
    public EAttackType AttackType { get { return attackType; } }
    // ���� �Ӽ�
    private ESinAffinities sinAffinities;
    public ESinAffinities SinAffinities { get {  return sinAffinities; } }

    private int defaultDamage = 0;      // �⺻ ������
    public int DefaultDamage { get { return defaultDamage; } }
    private int coinCount = 0;            // ���� ����
    public int CoinCount { get { return coinCount; } }
    private int coinDamage = 0;         // ���� ������
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
