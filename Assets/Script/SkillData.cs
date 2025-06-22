using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public string description;
    public float cooldown;

    // ��ų ȿ���� �����ϱ� ���� �޼���
    public virtual void Activate(GameObject user)
    {
        Debug.Log("�⺻ ��ų ȿ��: " + skillName);
    }
}
