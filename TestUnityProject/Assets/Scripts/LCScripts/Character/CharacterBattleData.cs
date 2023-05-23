using Battle;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleData : MonoBehaviour, IBattleFunction
{
    CharacterData characterData;

    int curHP = 0;
    int curMentality = 0;
    int curSpeed = 0;

    int staggerIdx = 0;             // ���� ��Ʈ���� ����
    int curStaggerCount = 0;     // ��Ʈ���� ���¶�� �� ����
    bool isStagger = false;        // �̹� �Ͽ� ��Ʈ���� ������ üũ

    public int CurSpeed {  get { return curSpeed; }  }

    public void Init()
    {
        characterData = new CharacterData();
        characterData.Init();

        curStaggerCount = 0;
    }

    // �̹� �� ����
    public void StartTurn()
    {
        // �ӵ� ���� ������ ����� �ִٸ� ���⼭ ó��
        int rand = Random.Range(-1, 2);
        curSpeed = characterData.mySpeed + rand;        // �ӵ� ����

        isStagger = false;
    }

    public void EndTurn()
    {
        // �̹� �Ͽ� ��Ʈ���� ���� �ƴ϶�� ����
        if(isStagger == false)
        {
            curStaggerCount = 0;
        }
    }

    public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx = 0)
    {
        // �Ӽ� ���� ����
        curHP -= characterData.GetDamage(damage, staggerIdx, coinSuccess);

        // ������� �߰� �������� �ִٸ� ���⼭ ����

        if(curHP < characterData.staggerThreshold[staggerIdx])
        {
            curStaggerCount++;
            staggerIdx++;
            isStagger = true;
        }
    }

    public List<SkillData> GetSkillData()
    {
        // ������ ��ų�� ����� �ϴ� ��찡 ������ ���⼭ ó��
        return characterData.GetSkillData();
    }

    public bool IsDead { get { return curHP <= 0; } }
    public bool IsPanic { get { return curMentality <= BattleInfo.Min_Mentality; } }
}
