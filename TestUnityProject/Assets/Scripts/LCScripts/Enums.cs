using System.Collections.Generic;

namespace Battle
{
    // 공격 타입
    // 참격, 관통, 타격
    public enum AttackType
    {
        Slash = 0,
        Pierce,
        Blunt
    }

    // 공격 속성
    // 분노 색욕 나태 탐식 우울 오만 질투
    public enum SinAffinities
    {
        Wrath = 0,
        Lust,
        Sloth,
        Glut,
        Gloom,
        Pride,
        Envy,
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
    }

    /// <summary>
    /// 데미지 정보
    /// </summary>
    public struct DamageInfo
    {
        public AttackType AttackType;
        public SinAffinities SinAffinities;
        public int Damage;
    }

    interface IBattleFunction
    {
        // 턴 시작 시 세팅
        public void StartTurn();
        // 턴 종료 시 세팅
        public void EndTurn();
        // 데미지 적용
        public void Damaged(DamageInfo damage, int partIdx = 0);
        public List<SkillData> GetSkillData();
    }
}