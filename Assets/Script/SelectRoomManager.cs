using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRoomManager : MonoBehaviour
{
    // ��ư�� �ε���(0, 1, 2 ��)�� �ѱ�� ����
    public void OnWeaponSelect(int index)
    {
        PlayerSelectData.selectedWeaponIndex = index;
        Debug.Log("������ ����: " + index);
    }

    public void OnSkillSelect(int index)
    {
        PlayerSelectData.selectedSkillIndex = index;
        Debug.Log("������ ��ų: " + index);
    }

    // "���� �Ϸ�" ��ư���� ȣ��
    public void OnConfirmButton()
    {
        SceneManager.LoadScene("TopViewMap_1"); // "GameScene"�� ���� ���� �÷��� �� �̸����� ����
    }
}
