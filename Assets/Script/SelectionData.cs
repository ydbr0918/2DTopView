using System.Collections.Generic;
using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static SelectionData Instance { get; private set; }

    [Header("무기 옵션")]
    public List<WeaponData> weaponOptions;
    [Header("스킬 옵션")]
    public List<SkillData> skillOptions;

    public int SelectedWeaponIndex { get; private set; }
    public WeaponData SelectedWeapon { get; private set; }

    public int SelectedSkillIndex { get; private set; }
    public SkillData SelectedSkill { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 인덱스로 무기·스킬을 설정하는 메서드
    public void SetSelectedWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponOptions.Count)
            Debug.LogWarning($"Invalid weapon index {idx}");
        SelectedWeaponIndex = Mathf.Clamp(idx, 0, weaponOptions.Count - 1);
        SelectedWeapon = weaponOptions[SelectedWeaponIndex];
    }

    public void SetSelectedSkill(int idx)
    {
        if (idx < 0 || idx >= skillOptions.Count)
            Debug.LogWarning($"Invalid skill index {idx}");
        SelectedSkillIndex = Mathf.Clamp(idx, 0, skillOptions.Count - 1);
        SelectedSkill = skillOptions[SelectedSkillIndex];
    }

    public void ResetSelection()
    {
        SelectedWeaponIndex = 0;
        SelectedSkillIndex = 0;
    }
}
