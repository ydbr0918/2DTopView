using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("��ų �⺻ ����")]
    public string skillName;     
    public Sprite icon;
    [TextArea] public string description;
    public float cooldown = 1f;


    public virtual void Activate(GameObject user)
    {
        Debug.Log($"[SkillData] �⺻ Activate(): {skillName}");
    }
}
