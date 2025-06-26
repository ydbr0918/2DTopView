using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLobbyManager : MonoBehaviour
{
   
    public void OnStartGame()
    {
        SaveManager.DeleteSave();                     
        SelectionData.Instance.ResetSelection();     
        SceneManager.LoadScene("SelectRoom");
    }

    
    public void OnContinueGame()
    {
        if (SaveManager.HasSave)
        {
            SaveManager.LoadGame();
            SceneManager.LoadScene("TopViewMap_1");
        }
        else
            Debug.LogWarning("�ҷ��� ������ �����ϴ�.");
    }

    
    public void OnQuitGame() => Application.Quit();
}