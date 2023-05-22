using Battle;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterData : MonoBehaviour
{
    // Ÿ�� ����
    public float[] attackTypeTolerance = new float[BattleInfo.Num_Type];
    // �Ӽ� ����
    public float[] sinAffinitiesTolerance = new float[BattleInfo.Num_Sin];
    // ��ų ������
    SkillData[] mySkills;

    public int mySpeed = 0;        // �ӵ�
    public int myHP = 0;            // ü��
    public int myMentality = 0;    // ���ŷ�

    public int[] staggerThreshold = new int[BattleInfo.Num_Stagger];     // ��Ʈ���� ����

    // ������� ĳ���� �����Ϳ� ���� �⺻ ���� �����;� ������ ���⿡���� ���� �������� ����
    public void Init()
    {
        int random = 0;
        // �������� 0.7~1.5f ���̰� �ǵ��� �Ѵ�.
        for (int i = 0; i < sinAffinitiesTolerance.Length; i++)
        {
            random = Random.Range(7, 16);
            sinAffinitiesTolerance[i] = random * 0.1f;
        }

        // 2, 3�� ����
        random = Random.Range(2, 4);
        mySkills = new SkillData[random];
        foreach (var skill in mySkills)
        {
            skill.Init();
        }

        random = Random.Range(BattleInfo.Min_Speed, BattleInfo.Max_Speed + 1);
        mySpeed = random;
        random = Random.Range(BattleInfo.Min_HP, BattleInfo.Max_HP + 1);
        myHP = random;
        myMentality = 0;

        // �ִ� ü�� �������� ����
        SetStagger(myHP);
    }

    public void SetStagger(int hp)
    {
        for (int i = 0; i < staggerThreshold.Length; i++)
        {
            staggerThreshold[i] = (int)(myHP * BattleInfo.StaggerHP[i]);
        }
    }

    /// <summary>
    /// ������ ���
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <param name="stagger"></param>
    /// <returns></returns>
    public int GetDamage(DamageInfo damageInfo, int stagger)
    {
        int result = damageInfo.Damage;

        result = (int)(result                                                              // ������
            * attackTypeTolerance[(int)damageInfo.AttackType]        // ���� Ÿ�� ����
            * sinAffinitiesTolerance[(int)damageInfo.SinAffinities]      // ���� �Ӽ� ����
            * BattleInfo.StaggerAttackRatio[stagger]);                                           // ��Ʈ���� ����

        return result;
    }

    /// <summary>
    /// �����ϰ� ��ų 2�� ��������
    /// </summary>
    /// <returns></returns>
    public List<SkillData> GetSkillData()
    {
        List<SkillData> result = new List<SkillData>();

        // 2�� ����
        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, mySkills.Length);
            result.Add(mySkills[rand]);
        }

        return result;
    }
}
