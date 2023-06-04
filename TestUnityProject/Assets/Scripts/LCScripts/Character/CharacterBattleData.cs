using Battle;
using UnityEngine;

public class CharacterBattleData : MonoBehaviour, IBattleFunction
{
    private CharacterData _characterData;

    private int _curHP = 0;
    private int _curMentality = 0;
    private int _curSpeed = 0;

    private int _staggerIdx = 0;             // ���� ��Ʈ���� ����
    private int _curStaggerCount = 0;     // ��Ʈ���� ���¶�� �� ����
    private bool _isStagger = false;        // �̹� �Ͽ� ��Ʈ���� ������ üũ

    public int CurSpeed { get { return _curSpeed; } }

    public void Init()
    {
        _characterData = new CharacterData();
        _characterData.Init();

        _curStaggerCount = 0;
    }

    /// <summary>
    /// ���� �佺 ���� ���� ����
    /// </summary>
    /// <returns></returns>
    private bool CoinToss
    {
        get
        {
            int max = 100;
            int defaultPercentage = 50;
            int rand = Random.Range(0, max);

            // �⺻ 50�� + ���ŷ� ��ġ (-45 ~ +45)
            return rand < (defaultPercentage + _curMentality);
        }
    }

    // �̹� �� ����
    public void StartTurn()
    {
        // �ӵ� ���� ������ ����� �ִٸ� ���⼭ ó��
        int rand = Random.Range(-1, 2);
        _curSpeed = _characterData.Speed + rand;        // �ӵ� ����

        _isStagger = false;
    }

    public void EndTurn()
    {
        // �̹� �Ͽ� ��Ʈ���� ���� �ƴ϶�� ����
        if (_isStagger == false)
        {
            _curStaggerCount = 0;
        }
    }

    public void Damaged(AttackInfo damage, int partIdx = 0)
    {
        // �Ӽ� ���� ����
        _curHP -= _characterData.GetDamage(damage, _curStaggerCount, CoinToss);

        // ������� �߰� �������� �ִٸ� ���⼭ ����

        if (_curHP < _characterData.StaggerThreshold[_staggerIdx])
        {
            _curStaggerCount++;
            _staggerIdx++;
            _isStagger = true;
        }
    }

    /// <summary>
    /// ���� �Ͽ� ����� ��ų ����
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public SkillBlock GetSkillData(int count = 2)
    {
        SkillBlock result = new SkillBlock();
        result.Skill = new SkillData[count];
        result.Character = this;

        // �д��� ��� ���� �Ұ�
        // ����ִ� ��ų�������� ����ó�� �ʿ�
        if (IsPanic) return result;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, _characterData.SkillCount);
            result.Skill[i] = _characterData.GetSkill(rand);
        }

        return result;
    }

    /// <summary>
    /// ��� üũ
    /// </summary>
    public bool IsDead { get { return _curHP <= 0; } }
    /// <summary>
    /// �д� üũ
    /// </summary>
    public bool IsPanic { get { return _curMentality <= Battle.CharacterInfo.Min_Mentality; } }
}
