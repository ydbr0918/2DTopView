using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static SelectionData Instance { get; private set; }

    public SkillData SelectedSkill { get; private set; }
    public WeaponData SelectedWeapon { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSelectedSkill(int idx, SkillData skill)
    {
        SelectedSkill = skill;
        Debug.Log($"[SelectionData] Skill #{idx} ¡æ {skill.skillName}");
    }

    public void SetSelectedWeapon(int idx, WeaponData weapon)
    {
        SelectedWeapon = weapon;
        Debug.Log($"[SelectionData] Weapon #{idx} ¡æ {weapon.weaponName}");
    }
}
