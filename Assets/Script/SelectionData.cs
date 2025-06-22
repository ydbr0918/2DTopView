// SelectionData.cs
using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static SelectionData Instance { get; private set; }

    // ���õ� ���⡤��ų �����Ϳ� �� �ε���
    public WeaponData SelectedWeapon { get; private set; }
    public int SelectedWeaponIndex { get; private set; }
    public SkillData SelectedSkill { get; private set; }
    public int SelectedSkillIndex { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���� ���� UI ���� ȣ��
    /// </summary>
    public void SetSelectedWeapon(int index, WeaponData weapon)
    {
        SelectedWeaponIndex = index;
        SelectedWeapon = weapon;
        Debug.Log($"[SelectionData] ���õ� ����: {weapon.weaponName} (�ε��� {index})");
    }

    /// <summary>
    /// ��ų ���� UI ���� ȣ��
    /// </summary>
    public void SetSelectedSkill(int index, SkillData skill)
    {
        SelectedSkillIndex = index;
        SelectedSkill = skill;
        Debug.Log($"[SelectionData] ���õ� ��ų: {skill.skillName} (�ε��� {index})");
    }
}
