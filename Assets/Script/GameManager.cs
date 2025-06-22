using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        WeaponData weapon = SelectionData.Instance.selectedWeapon;
        SkillData skill = SelectionData.Instance.selectedSkill;

        Debug.Log("선택된 무기: " + weapon.weaponName + ", 데미지: " + weapon.damage);
        Debug.Log("선택된 스킬: " + skill.skillName + ", 설명: " + skill.description);

        // 여기에서 실제 무기 오브젝트나 스킬 시스템에 적용
        // 예: player.EquipWeapon(weapon);
        //     skillSystem.Apply(skill);
    }
}
