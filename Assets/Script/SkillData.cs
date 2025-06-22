using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public string description;
    public float cooldown;

    // 스킬 효과를 실행하기 위한 메서드
    public virtual void Activate(GameObject user)
    {
        Debug.Log("기본 스킬 효과: " + skillName);
    }
}
