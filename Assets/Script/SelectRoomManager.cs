using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRoomManager : MonoBehaviour
{
    // 버튼이 인덱스(0, 1, 2 등)를 넘기게 연결
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

    // "선택 완료" 버튼에서 호출
    public void OnConfirmButton()
    {
        SceneManager.LoadScene("TopViewMap_1"); // "GameScene"을 본인 게임 플레이 씬 이름으로 변경
    }
}
