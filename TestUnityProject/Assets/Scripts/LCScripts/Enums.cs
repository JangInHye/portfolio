using System;

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

    // 캐릭터 데이터
    class CharacterInfo
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
    /// 공격 정보
    /// </summary>
    public class AttackInfo
    {
        public CharacterBattleData Attacker;
        public CharacterBattleData Victim;
        public SkillData AttackerSkill;
        public SkillData VictimSkill;
        public int BeforeDamage;              // 이전 데미지 + 추가코인 데미지 계산
        public int CoinCount;                   // 공격횟수 줄어들 수도 있음.
        public int Speed;                        // 합을 진행하면 낮은 쪽을 따라감. 기본은 공격자 속도
    }

    // 스킬 UI 표기 및 현재 턴의 사용할 수 있는 스킬 세팅
    public struct SkillBlock
    {
        public SkillData[] Skill;
        public CharacterBattleData Character;
        public bool IsAttacked;         // 공격 명령이 내려졌는 지 체크
    }

    // 전투에 필요한 함수
    interface IBattleFunction
    {
        // 턴 시작 시 세팅
        public void StartTurn();
        // 턴 종료 시 세팅
        public void EndTurn();
        // 데미지 적용
        public void Damaged(AttackInfo damage, int partIdx = 0);
        public bool IsDead { get { return false; } }
    }
}