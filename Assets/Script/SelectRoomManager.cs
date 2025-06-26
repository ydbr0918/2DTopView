using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRoomManager : MonoBehaviour
{

    public void OnWeaponSelect(int index)
    {
        PlayerSelectData.selectedWeaponIndex = index;
        Debug.Log("선택한 무기: " + index);
    }

    public void OnSkillSelect(int index)
    {
        PlayerSelectData.selectedSkillIndex = index;
        Debug.Log("선택한 스킬: " + index);
    }


    public void OnConfirmButton()
    {
        SceneManager.LoadScene("TopViewMap_1");

    }
}