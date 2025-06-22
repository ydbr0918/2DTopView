using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 소문자가 아니라 대문자 프로퍼티로!
        WeaponData weapon = SelectionData.Instance.SelectedWeapon;
        SkillData skill = SelectionData.Instance.SelectedSkill;

        if (weapon != null)
        {
            Debug.Log($"선택된 무기: {weapon.weaponName}, 발사 속도: {weapon.fireRate}, 탄발수: {weapon.bulletPerShot}");
        }
        else
        {
            Debug.LogWarning("무기가 선택되지 않았습니다!");
        }

        if (skill != null)
        {
            Debug.Log($"선택된 스킬: {skill.skillName}, 설명: {skill.description}");
        }
        else
        {
            Debug.LogWarning("스킬이 선택되지 않았습니다!");
        }

        // 예: 플레이어에게 적용
        // var player = FindObjectOfType<Player>();
        // if (player != null && weapon != null) player.EquipWeapon(weapon);
        // if (player != null && skill  != null) skill.Activate(player.gameObject);
    }
}
