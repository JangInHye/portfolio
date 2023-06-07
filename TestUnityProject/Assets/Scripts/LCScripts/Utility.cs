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

    // ĳ���� ������
    struct CharacterInfo
    {
        public const int Num_Type = 3;
        public const int Num_Sin = 7;
        public const int Num_Stagger = 3;
        public static float[] StaggerHP = new float[] { 0.7f, 0.5f, 0.3f };
        public static float[] StaggerAttackRatio = new float[] { 1.0f, 1.2f, 1.4f, 1.6f };

        public const int Min_DefaultDamage = 7;
        public const int Min_CoinDamage = 1;
        public const int Min_CoinCount = 1;
        public const int Min_Speed = 2;
        public const int Min_HP = 120;
        public const int Min_Mentality = -45;

        public const int Max_DefaultDamage = 9;
        public const int Max_CoinDamage = 4;
        public const int Max_CoinCount = 5;
        public const int Max_Speed = 7;
        public const int Max_HP = 150;
        public const int Max_Mentality = 45;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public class AttackInfo
    {
        public SkillBlock Attacker;
        public SkillBlock Victim;
        public int AttackSkillIndex;
        public int VictimSkillIndex;

        public SkillData AttackerSkill
        {
            get { return Attacker.Skill[AttackSkillIndex]; }
        }
        public SkillData VictimSkill
        {
            get { return VictimSkillIndex == -1 ? null : Victim.Skill[VictimSkillIndex]; }
        }

        public int BeforeDamage;              // ���� ������ + �߰����� ������ ���
        public int CoinCount;                   // ����Ƚ�� �پ�� ���� ����.
        public int Speed;                        // ���� �����ϸ� ���� ���� ����. �⺻�� ������ �ӵ�
    }


    // ������ �ʿ��� �Լ�
    interface IBattleFunction
    {
        // �� ���� �� ����
        public void StartTurn();
        // �� ���� �� ����
        public void EndTurn();
        // ������ ����
        public void Damaged(AttackInfo damage, int partIdx = 0);
        public bool IsDead { get { return false; } }
    }
}