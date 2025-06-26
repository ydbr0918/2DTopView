using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUIManager : MonoBehaviour
{

    public void OnRetryButton()
    {
    
        SceneManager.LoadScene("MainLobby");
    }


    public void OnQuitButton()
    {
        Application.Quit();


    }
}
