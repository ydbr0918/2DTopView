using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        WeaponData weapon = SelectionData.Instance.selectedWeapon;
        SkillData skill = SelectionData.Instance.selectedSkill;

        Debug.Log("���õ� ����: " + weapon.weaponName + ", ������: " + weapon.damage);
        Debug.Log("���õ� ��ų: " + skill.skillName + ", ����: " + skill.description);

        // ���⿡�� ���� ���� ������Ʈ�� ��ų �ý��ۿ� ����
        // ��: player.EquipWeapon(weapon);
        //     skillSystem.Apply(skill);
    }
}
