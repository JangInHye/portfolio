using Battle;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    // 타입 내성
    private float[] _attackTypeTolerance = new float[Battle.CharacterInfo.Num_Type];
    // 속성 내성
    private float[] _sinAffinitiesTolerance = new float[Battle.CharacterInfo.Num_Sin];
    // 스킬 ID만 갖고 있는다.
    int[] _mySkills;
    public int SkillCount { get { return _mySkills == null ? 0 : _mySkills.Length; } }
    public SkillData GetSkill(int idx)
    {
        return SkillDataHelper.Instance.GetSkillData(idx);
    }

    private int _mySpeed = 0;        // 속도
    public int Speed { get { return _mySpeed; } }
    private int _myHP = 0;            // 체력
    private int _myMentality = 0;    // 정신력

    private int[] _staggerThreshold = new int[Battle.CharacterInfo.Num_Stagger];     // 흐트러짐 기준
    public int[] StaggerThreshold { get { return _staggerThreshold; } }

    // 원래라면 캐릭터 데이터에 따라 기본 값을 가져와야 하지만 여기에서는 전부 랜덤으로 세팅
    public void Init()
    {
        int random = 0;
        // 랜덤으로 0.7~1.5f 사이가 되도록 한다.
        for (int i = 0; i < _sinAffinitiesTolerance.Length; i++)
        {
            random = Random.Range(7, 16);
            _sinAffinitiesTolerance[i] = random * 0.1f;
        }

        // 2, 3개 랜덤
        random = Random.Range(2, 4);
        _mySkills = new int[random];
        for (int i = 0; i < _mySkills.Length; i++)
        {
            random = Random.Range(0, SkillDataHelper.MAX_SKILL_COUNT);
            _mySkills[i] = random;
        }

        random = Random.Range(Battle.CharacterInfo.Min_Speed, Battle.CharacterInfo.Max_Speed + 1);
        _mySpeed = random;
        random = Random.Range(Battle.CharacterInfo.Min_HP, Battle.CharacterInfo.Max_HP + 1);
        _myHP = random;
        _myMentality = 0;

        // 최대 체력 기준으로 설정
        SetStagger(_myHP);
    }

    public void SetStagger(int hp)
    {
        for (int i = 0; i < _staggerThreshold.Length; i++)
        {
            _staggerThreshold[i] = (int)(_myHP * Battle.CharacterInfo.StaggerHP[i]);
        }
    }

    /// <summary>
    /// 데미지 계산
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <param name="stagger"></param>
    /// <returns></returns>
    public int GetDamage(AttackInfo damageInfo, int stagger, bool coinToss)
    {
        int result = damageInfo.AttackerSkill.Damage(damageInfo.BeforeDamage, coinToss);

        result = (int)(result                                                   // 데미지
            * _attackTypeTolerance[(int)damageInfo.AttackerSkill.AttackType]        // 공격 타입 내성
            * _sinAffinitiesTolerance[(int)damageInfo.AttackerSkill.SinAffinities]      // 공격 속성 내성
            * Battle.CharacterInfo.StaggerAttackRatio[stagger]);                    // 흐트러짐 여부

        return result;
    }
}
