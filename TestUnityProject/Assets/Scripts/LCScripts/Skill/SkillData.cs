using Battle;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    private int _index = -1;
    public int Index { get { return _index; } }

    // ���� Ÿ��
    private EAttackType _attackType;
    public EAttackType AttackType { get { return _attackType; } }
    // ���� �Ӽ�
    private ESinAffinities _sinAffinities;
    public ESinAffinities SinAffinities { get {  return _sinAffinities; } }

    private int _defaultDamage = 0;      // �⺻ ������
    public int DefaultDamage { get { return _defaultDamage; } }
    private int _coinCount = 0;            // ���� ����
    public int CoinCount { get { return _coinCount; } }
    private int _coinDamage = 0;         // ���� ������
    public int CoinDamage { get { return _coinDamage; } }

    public void Init(int idx)
    {
        _index = idx;

        int random = Random.Range(0, System.Enum.GetValues(typeof(Battle.EAttackType)).Length);
        _attackType = (Battle.EAttackType)random;

        random = Random.Range(0, System.Enum.GetValues(typeof(Battle.ESinAffinities)).Length);
        _sinAffinities = (Battle.ESinAffinities)random;

        random = Random.Range(Battle.CharacterInfo.Min_DefaultDamage, Battle.CharacterInfo.Max_DefaultDamage + 1);
        _defaultDamage = random;
        random = Random.Range(Battle.CharacterInfo.Min_CoinCount, Battle.CharacterInfo.Max_CoinCount + 1);
        _coinCount = random;
        random = Random.Range(Battle.CharacterInfo.Min_CoinDamage, Battle.CharacterInfo.Max_CoinDamage + 1);
        _coinDamage = random;
    }

    // ������ ���� ������ ������ �߰� ���
    public int Damage(int beforeDamage, bool coinSuccess)
    {
        int result = beforeDamage;

        // ������ ���� ������ �ִٸ� ���⼭ ó��
        // �����佺�� �����ϸ� ���ε����� �߰�
        // ���ε������� ������ �� �ִ�.
        result += coinSuccess ? CoinDamage : 0;

        return result;
    }
}
