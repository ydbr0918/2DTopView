using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    public string skillName;     
    public Sprite icon;
    [TextArea] public string description;
    public float cooldown = 1f;


    public virtual void Activate(GameObject user)
    {
        Debug.Log($"[SkillData] 기본 Activate(): {skillName}");
    }
}
