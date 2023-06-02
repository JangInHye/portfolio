using Battle;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    private int _index = -1;
    public int Index { get { return _index; } }

    // 공격 타입
    private EAttackType _attackType;
    public EAttackType AttackType { get { return _attackType; } }
    // 공격 속성
    private ESinAffinities _sinAffinities;
    public ESinAffinities SinAffinities { get {  return _sinAffinities; } }

    private int _defaultDamage = 0;      // 기본 데미지
    public int DefaultDamage { get { return _defaultDamage; } }
    private int _coinCount = 0;            // 코인 갯수
    public int CoinCount { get { return _coinCount; } }
    private int _coinDamage = 0;         // 코인 데미지
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

    // 코인을 던질 때마다 데미지 추가 계산
    public int Damage(int beforeDamage, bool coinSuccess)
    {
        int result = beforeDamage;

        // 데미지 증가 버프가 있다면 여기서 처리
        // 코인토스에 성공하면 코인데미지 추가
        // 코인데미지가 음수일 수 있다.
        result += coinSuccess ? CoinDamage : 0;

        return result;
    }
}
