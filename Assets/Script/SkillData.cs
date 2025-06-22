using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    public string skillName;      // AmmoBoostSkill 에서도 이걸 씁니다
    public Sprite icon;
    [TextArea] public string description;
    public float cooldown = 1f;

    /// <summary>
    /// 스킬 발동 함수. AmmoBoostSkill 등에서 override 합니다.
    /// </summary>
    public virtual void Activate(GameObject user)
    {
        Debug.Log($"[SkillData] 기본 Activate(): {skillName}");
    }
}
