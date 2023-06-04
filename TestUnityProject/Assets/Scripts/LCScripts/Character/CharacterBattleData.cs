using Battle;
using UnityEngine;

public class CharacterBattleData : MonoBehaviour, IBattleFunction
{
    private CharacterData _characterData;

    private int _curHP = 0;
    private int _curMentality = 0;
    private int _curSpeed = 0;

    private int _staggerIdx = 0;             // 현재 흐트러짐 기준
    private int _curStaggerCount = 0;     // 흐트러진 상태라면 그 정도
    private bool _isStagger = false;        // 이번 턴에 흐트러진 것인지 체크

    public int CurSpeed { get { return _curSpeed; } }

    public void Init()
    {
        _characterData = new CharacterData();
        _characterData.Init();

        _curStaggerCount = 0;
    }

    /// <summary>
    /// 코인 토스 성공 실패 여부
    /// </summary>
    /// <returns></returns>
    private bool CoinToss
    {
        get
        {
            int max = 100;
            int defaultPercentage = 50;
            int rand = Random.Range(0, max);

            // 기본 50퍼 + 정신력 수치 (-45 ~ +45)
            return rand < (defaultPercentage + _curMentality);
        }
    }

    // 이번 턴 세팅
    public void StartTurn()
    {
        // 속도 관련 버프나 디버프 있다면 여기서 처리
        int rand = Random.Range(-1, 2);
        _curSpeed = _characterData.Speed + rand;        // 속도 랜덤

        _isStagger = false;
    }

    public void EndTurn()
    {
        // 이번 턴에 흐트러진 것이 아니라면 해제
        if (_isStagger == false)
        {
            _curStaggerCount = 0;
        }
    }

    public void Damaged(AttackInfo damage, int partIdx = 0)
    {
        // 속성 내성 적용
        _curHP -= _characterData.GetDamage(damage, _curStaggerCount, CoinToss);

        // 디버프나 추가 데미지가 있다면 여기서 적용

        if (_curHP < _characterData.StaggerThreshold[_staggerIdx])
        {
            _curStaggerCount++;
            _staggerIdx++;
            _isStagger = true;
        }
    }

    /// <summary>
    /// 현재 턴에 사용할 스킬 묶음
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public SkillBlock GetSkillData(int count = 2)
    {
        SkillBlock result = new SkillBlock();
        result.Skill = new SkillData[count];
        result.Character = this;

        // 패닉일 경우 공격 불가
        // 비어있는 스킬데이터의 예외처리 필요
        if (IsPanic) return result;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, _characterData.SkillCount);
            result.Skill[i] = _characterData.GetSkill(rand);
        }

        return result;
    }

    /// <summary>
    /// 사망 체크
    /// </summary>
    public bool IsDead { get { return _curHP <= 0; } }
    /// <summary>
    /// 패닉 체크
    /// </summary>
    public bool IsPanic { get { return _curMentality <= Battle.CharacterInfo.Min_Mentality; } }
}
