using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLobbyManager : MonoBehaviour
{
    // 새 게임 시작
    public void OnStartGame()
    {
        SaveManager.DeleteSave();                     // ← 이 한 줄 추가!
        SelectionData.Instance.ResetSelection();      // ← 선택 인덱스도 초기화
        SceneManager.LoadScene("SelectRoom");
    }

    // 저장된 게임 이어하기
    public void OnContinueGame()
    {
        if (SaveManager.HasSave)
        {
            SaveManager.LoadGame();
            SceneManager.LoadScene("TopViewMap_1");
        }
        else
            Debug.LogWarning("불러올 저장이 없습니다.");
    }

    // 완전 종료
    public void OnQuitGame() => Application.Quit();
}