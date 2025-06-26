using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRoomManager : MonoBehaviour
{

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


    public void OnConfirmButton()
    {
        SceneManager.LoadScene("TopViewMap_1");

    }
}