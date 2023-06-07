public class SkillDataHelper
{
    private static SkillDataHelper _instance;
    public static SkillDataHelper Instance
    {
        get
        {
            if( _instance == null )
                _instance = new SkillDataHelper();
            return _instance;
        }
    }

    // ��ų �ִ� ����
    public const int MAX_SKILL_COUNT = 20;

    private SkillData[] _skillDataArray;
    public SkillData GetSkillData(int idx)
    {
        if (_skillDataArray.Length == 0 || _skillDataArray.Length <= idx || idx < 0) return null;

        return _skillDataArray[idx]; 
    }

    /// <summary>
    /// �̹� ���ӿ� ����� ��ų ������ �ʱ�ȭ
    /// ������� ��Ʈ �����͸� �о�;� �Ѵ�.
    /// </summary>
    public void Init()
    {
        _skillDataArray = new SkillData[MAX_SKILL_COUNT];

        int index = 0;
        foreach(SkillData skillData in _skillDataArray)
        {
            skillData.Init(index++);
        }
    }
}
