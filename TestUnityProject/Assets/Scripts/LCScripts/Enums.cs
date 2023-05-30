using System.Collections.Generic;

namespace Battle
{
    // ���� Ÿ��
    // ����, ����, Ÿ��
    public enum EAttackType
    {
        Slash = 0,
        Pierce,
        Blunt
    }

    // ���� �Ӽ�
    // �г� ���� ���� Ž�� ��� ���� ����
    public enum ESinAffinities
    {
        Wrath = 0,
        Lust,
        Sloth,
        Glut,
        Gloom,
        Pride,
        Envy,
    }

    // ���� ����
    public enum EBattleState
    {
        None,
        StartTurn,              // �� ����
        WaitCommand,        // ���� ���
        StartAttack,            // ���� ����
        EndTurn,               // �� ����
        Victory,                 // �¸�
        Defeat,                 // �й�
    }

    class BattleInfo
    {
        public static int Num_Type = 3;
        public static int Num_Sin = 7;
        public static int Num_Stagger = 3;
        public static float[] StaggerHP = new float[] { 0.7f, 0.5f, 0.3f };
        public static float[] StaggerAttackRatio = new float[] { 1.0f, 1.2f, 1.4f, 1.6f };

        public static int Min_DefaultDamage = 7;
        public static int Min_CoinDamage = 1;
        public static int Min_CoinCount = 1;
        public static int Min_Speed = 2;
        public static int Min_HP = 120;
        public static int Min_Mentality = -45;

        public static int Max_DefaultDamage = 9;
        public static int Max_CoinDamage = 4;
        public static int Max_CoinCount = 5;
        public static int Max_Speed = 7;
        public static int Max_HP = 150;
        public static int Max_Mentality = 45;

        // ������ ���� ������ ������ �߰� ���
        public static int Damage(SkillData skill, int beforeDamage, bool coinSuccess)
        {
            int result = beforeDamage;

            // ������ ���� ������ �ִٸ� ���⼭ ó��
            result += skill.CoinDamage;

            return result;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public struct AttackInfo
    {
        public CharacterBattleData Attacker;
        public CharacterBattleData Victim;
        public SkillData AttackerSkill;
        public SkillData VictimSkill;
        public int BeforeDamage;              // ���� ������ + �߰����� ������ ���
        public int CoinCount;                   // ����Ƚ�� �پ�� ���� ����.
        public int Speed;                        // ������ ���� �ϴ��Ŀ� ���� �ӵ� �޶���.
    }

    // ��ų UI ǥ�� �� ���� ���� ����� �� �ִ� ��ų ����
    public struct SkillBlock
    {
        public SkillData[] Skill;
        public CharacterBattleData Character;
        public bool IsAttacked;         // ���� ����� �������� �� üũ
    }

    interface IBattleFunction
    {
        // �� ���� �� ����
        public void StartTurn();
        // �� ���� �� ����
        public void EndTurn();
        // ������ ����
        public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx = 0);
        public bool IsDead { get { return false; } }
    }
}