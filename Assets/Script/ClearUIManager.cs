using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUIManager : MonoBehaviour
{
    /// <summary>
    /// “처음으로” 버튼에 연결
    /// </summary>
    public void OnRetryButton()
    {
        // MainLobby 씬 이름이 “MainLobby” 라고 가정
        SceneManager.LoadScene("MainLobby");
    }

    /// <summary>
    /// “나가기” 버튼에 연결
    /// </summary>
    public void OnQuitButton()
    {
        // 에디터에서는 동작하지 않지만, 빌드된 앱에서는 앱 종료
        Application.Quit();

        // (디버그 용) 에디터에서 종료 명령을 보고 싶으면 아래 주석 해제
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }
}
