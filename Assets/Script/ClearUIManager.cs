using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUIManager : MonoBehaviour
{
    /// <summary>
    /// ��ó�����Ρ� ��ư�� ����
    /// </summary>
    public void OnRetryButton()
    {
        // MainLobby �� �̸��� ��MainLobby�� ��� ����
        SceneManager.LoadScene("MainLobby");
    }

    /// <summary>
    /// �������⡱ ��ư�� ����
    /// </summary>
    public void OnQuitButton()
    {
        // �����Ϳ����� �������� ������, ����� �ۿ����� �� ����
        Application.Quit();

        // (����� ��) �����Ϳ��� ���� ����� ���� ������ �Ʒ� �ּ� ����
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }
}
