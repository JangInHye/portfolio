using System.Collections.Generic;

namespace Battle
{
    // 공격 타입
    // 참격, 관통, 타격
    public enum EAttackType
    {
        Slash = 0,
        Pierce,
        Blunt
    }

    // 공격 속성
    // 분노 색욕 나태 탐식 우울 오만 질투
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

    // 전투 상태
    public enum EBattleState
    {
        None,
        StartTurn,              // 턴 시작
        WaitCommand,        // 조작 대기
        StartAttack,            // 공격 시작
        EndTurn,               // 턴 종료
        Victory,                 // 승리
        Defeat,                 // 패배
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

        // 코인을 던질 때마다 데미지 추가 계산
        public static int Damage(SkillData skill, int beforeDamage, bool coinSuccess)
        {
            int result = beforeDamage;

            // 데미지 증가 버프가 있다면 여기서 처리
            result += skill.CoinDamage;

            return result;
        }
    }

    /// <summary>
    /// 공격 정보
    /// </summary>
    public struct AttackInfo
    {
        public CharacterBattleData Attacker;
        public CharacterBattleData Victim;
        public SkillData AttackerSkill;
        public SkillData VictimSkill;
        public int BeforeDamage;              // 이전 데미지 + 추가코인 데미지 계산
        public int CoinCount;                   // 공격횟수 줄어들 수도 있음.
        public int Speed;                        // 누구랑 합을 하느냐에 따라 속도 달라짐.
    }

    // 스킬 UI 표기 및 현재 턴의 사용할 수 있는 스킬 세팅
    public struct SkillBlock
    {
        public SkillData[] Skill;
        public CharacterBattleData Character;
        public bool IsAttacked;         // 공격 명령이 내려졌는 지 체크
    }

    interface IBattleFunction
    {
        // 턴 시작 시 세팅
        public void StartTurn();
        // 턴 종료 시 세팅
        public void EndTurn();
        // 데미지 적용
        public void Damaged(AttackInfo damage, bool coinSuccess, int partIdx = 0);
        public bool IsDead { get { return false; } }
    }
}