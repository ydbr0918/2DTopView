// SelectionData.cs
using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static SelectionData Instance { get; private set; }

    // 선택된 무기·스킬 데이터와 그 인덱스
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
    /// 무기 선택 UI 에서 호출
    /// </summary>
    public void SetSelectedWeapon(int index, WeaponData weapon)
    {
        SelectedWeaponIndex = index;
        SelectedWeapon = weapon;
        Debug.Log($"[SelectionData] 선택된 무기: {weapon.weaponName} (인덱스 {index})");
    }

    /// <summary>
    /// 스킬 선택 UI 에서 호출
    /// </summary>
    public void SetSelectedSkill(int index, SkillData skill)
    {
        SelectedSkillIndex = index;
        SelectedSkill = skill;
        Debug.Log($"[SelectionData] 선택된 스킬: {skill.skillName} (인덱스 {index})");
    }
}
