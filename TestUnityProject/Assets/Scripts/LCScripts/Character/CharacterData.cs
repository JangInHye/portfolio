using Battle;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    // Ÿ�� ����
    private float[] _attackTypeTolerance = new float[Battle.CharacterInfo.Num_Type];
    // �Ӽ� ����
    private float[] _sinAffinitiesTolerance = new float[Battle.CharacterInfo.Num_Sin];
    // ��ų ID�� ���� �ִ´�.
    int[] _mySkills;
    public int SkillCount { get { return _mySkills == null ? 0 : _mySkills.Length; } }
    public SkillData GetSkill(int idx)
    {
        return SkillDataHelper.Instance.GetSkillData(idx);
    }

    private int _mySpeed = 0;        // �ӵ�
    public int Speed { get { return _mySpeed; } }
    private int _myHP = 0;            // ü��
    private int _myMentality = 0;    // ���ŷ�

    private int[] _staggerThreshold = new int[Battle.CharacterInfo.Num_Stagger];     // ��Ʈ���� ����
    public int[] StaggerThreshold { get { return _staggerThreshold; } }

    // ������� ĳ���� �����Ϳ� ���� �⺻ ���� �����;� ������ ���⿡���� ���� �������� ����
    public void Init()
    {
        int random = 0;
        // �������� 0.7~1.5f ���̰� �ǵ��� �Ѵ�.
        for (int i = 0; i < _sinAffinitiesTolerance.Length; i++)
        {
            random = Random.Range(7, 16);
            _sinAffinitiesTolerance[i] = random * 0.1f;
        }

        // 2, 3�� ����
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

        // �ִ� ü�� �������� ����
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
    /// ������ ���
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <param name="stagger"></param>
    /// <returns></returns>
    public int GetDamage(AttackInfo damageInfo, int stagger, bool coinToss)
    {
        int result = damageInfo.AttackerSkill.Damage(damageInfo.BeforeDamage, coinToss);

        result = (int)(result                                                   // ������
            * _attackTypeTolerance[(int)damageInfo.AttackerSkill.AttackType]        // ���� Ÿ�� ����
            * _sinAffinitiesTolerance[(int)damageInfo.AttackerSkill.SinAffinities]      // ���� �Ӽ� ����
            * Battle.CharacterInfo.StaggerAttackRatio[stagger]);                    // ��Ʈ���� ����

        return result;
    }
}
