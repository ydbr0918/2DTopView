using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("��ų �⺻ ����")]
    public string skillName;      // AmmoBoostSkill ������ �̰� ���ϴ�
    public Sprite icon;
    [TextArea] public string description;
    public float cooldown = 1f;

    /// <summary>
    /// ��ų �ߵ� �Լ�. AmmoBoostSkill ��� override �մϴ�.
    /// </summary>
    public virtual void Activate(GameObject user)
    {
        Debug.Log($"[SkillData] �⺻ Activate(): {skillName}");
    }
}
