using Battle;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour, IBattleFunction
{
    private CharacterBattleData[] _partsArray;
    private bool _isPartDestroy = false;     // 부위파괴

    private List<SkillBlock> _skillBlockList =  new List<SkillBlock>();

    // 본체
    private CharacterBattleData _bodyData;

    public void Init()
    {
        _bodyData = new CharacterBattleData();
        _bodyData.Init();

        // 2~4개 정도의 부위를 랜덤으로 가짐
        int random = Random.Range(2, 5);
        _partsArray = new CharacterBattleData[random];
        foreach (var part in _partsArray)
        {
            part.Init();
        }

        // 스킬 데이터 초기화
        foreach (var part in _partsArray)
        {
            _skillBlockList.Add(new SkillBlock(part, 1));
        }
        // 마지막 스킬은 본체로 고정
        _skillBlockList.Add(new SkillBlock(_bodyData));
    }

    public void StartTurn()
    {
        foreach (var part in _partsArray)
        {
            part.StartTurn();

            // 디버프 처리
        }
    }

    public void EndTurn()
    {
        foreach (var part in _partsArray)
        {
            part.EndTurn();

            // 디버프 감소 및 처리
        }
    }

    public void Damaged(AttackInfo damage, int partIdx)
    {
        // 부위 외에 공격은 본체로 간다.
        if (partIdx < 0 || partIdx >= _partsArray.Length || _partsArray[partIdx] == null)
        {
            _bodyData.Damaged(damage);
            return;
        }

        _partsArray[partIdx].Damaged(damage, partIdx);
        // 부위 파괴
        if (_partsArray[partIdx].IsDead)
        {
            _isPartDestroy = true;
        }
    }

    /// <summary>
    /// 각 부위별로 1개의 스킬 + 본체 1개 고정
    /// </summary>
    /// <returns></returns>
    public List<SkillBlock> SetSkillData()
    {
        if (_isPartDestroy)
        {
            _isPartDestroy = false;

            // 부위 파괴 시 한 턴간 전투 불가능
            return null;
        }

        foreach (var skillBlock in _skillBlockList)
        {
            // 본체는 넘어간다.
            if (skillBlock.Character == _bodyData) continue;

            // 파괴된 부위는 전투 불가능
            if (skillBlock.Character.IsDead)
            {
                // 비어있는 스킬데이터의 경우 부위 공격으로 대체한다.
                skillBlock.Skill = null;
                continue;
            }

            skillBlock.Skill = skillBlock.Character.GetSkillData(1);
        }

        // 강제로 스킬을 띄워야 하는 경우가 있으면 여기서 처리

        return _skillBlockList;
    }

    public bool IsDead { get { return _bodyData.IsDead; } }
}
