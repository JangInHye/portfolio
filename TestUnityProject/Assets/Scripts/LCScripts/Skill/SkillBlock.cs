// ��ų UI ǥ�� �� ���� ���� ����� �� �ִ� ��ų ����
public class SkillBlock
{
    public SkillData[] Skill;
    public CharacterBattleData Character;
    public bool IsAttacked;         // ���� ����� �������� �� üũ

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
    /// ������ �ִ� ��ų�� ���ο� ��ų�� ����
    /// </summary>
    /// <param name="index"></param>
    public void UseSkill(int index)
    {
        // ������ �ϳ��� ��ų�� ������ ��� �� �ǵڿ� ���ο� ��ų�� �־��ش�.
        for (int i = index; i < Skill.Length - 1; i++)
        {
            Skill[i] = Skill[i + 1];
        }
        Skill[Skill.Length - 1] = Character.GetSkillData();
    }
}
