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

    // 스킬 최대 갯수
    public const int MAX_SKILL_COUNT = 20;

    private SkillData[] _skillDataArray;
    public SkillData GetSkillData(int idx)
    {
        if (_skillDataArray.Length == 0 || _skillDataArray.Length <= idx || idx < 0) return null;

        return _skillDataArray[idx]; 
    }

    /// <summary>
    /// 이번 게임에 사용할 스킬 데이터 초기화
    /// 원래라면 시트 데이터를 읽어와야 한다.
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
