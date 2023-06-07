// 스킬 UI 표기 및 현재 턴의 사용할 수 있는 스킬 세팅
public class SkillBlock
{
    public SkillData[] Skill;
    public CharacterBattleData Character;
    public bool IsAttacked;         // 공격 명령이 내려졌는 지 체크

    public SkillBlock(CharacterBattleData character)
    {
        Character = character;
        Skill = null;
        IsAttacked = false;
    }

    public SkillBlock(CharacterBattleData character, int count)
    {
        Character = character;
        Skill = Character.GetSkillData(count);
        IsAttacked = false;
    }

    /// <summary>
    /// 기존에 있던 스킬을 새로운 스킬로 변경
    /// </summary>
    /// <param name="index"></param>
    public void UseSkill(int index)
    {
        // 앞으로 하나씩 스킬을 앞으로 당긴 후 맨뒤에 새로운 스킬을 넣어준다.
        for (int i = index; i < Skill.Length - 1; i++)
        {
            Skill[i] = Skill[i + 1];
        }
        Skill[Skill.Length - 1] = Character.GetSkillData();
    }
}
