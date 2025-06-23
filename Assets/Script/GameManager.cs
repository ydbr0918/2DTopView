using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // ���õ� ���⡤��ų ���� �о���� (�빮�� ������Ƽ ���)
        WeaponData weapon = SelectionData.Instance.SelectedWeapon;
        SkillData skill = SelectionData.Instance.SelectedSkill;

        // ���� ���� ���
        if (weapon != null)
        {
            Debug.Log($"���õ� ����: {weapon.weaponName}, �߻� �ӵ�: {weapon.fireRate}, ź�߼�: {weapon.bulletPerShot}");
        }
        else
        {
            Debug.LogWarning("���Ⱑ ���õ��� �ʾҽ��ϴ�!");
        }

        // ��ų ���� ���
        if (skill != null)
        {
            Debug.Log($"���õ� ��ų: {skill.skillName}, ����: {skill.description}");
        }
        else
        {
            Debug.LogWarning("��ų�� ���õ��� �ʾҽ��ϴ�!");
        }

    }
}
