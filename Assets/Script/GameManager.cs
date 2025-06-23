using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 선택된 무기·스킬 정보 읽어오기 (대문자 프로퍼티 사용)
        WeaponData weapon = SelectionData.Instance.SelectedWeapon;
        SkillData skill = SelectionData.Instance.SelectedSkill;

        // 무기 정보 출력
        if (weapon != null)
        {
            Debug.Log($"선택된 무기: {weapon.weaponName}, 발사 속도: {weapon.fireRate}, 탄발수: {weapon.bulletPerShot}");
        }
        else
        {
            Debug.LogWarning("무기가 선택되지 않았습니다!");
        }

        // 스킬 정보 출력
        if (skill != null)
        {
            Debug.Log($"선택된 스킬: {skill.skillName}, 설명: {skill.description}");
        }
        else
        {
            Debug.LogWarning("스킬이 선택되지 않았습니다!");
        }

    }
}
