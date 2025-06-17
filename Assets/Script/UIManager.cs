using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject HelpPanel;
    public void SelectButtonAction()
    {
        SceneManager.LoadScene("SelectRoom");
    }
    public void GameStartButtonAction()
    {
        SceneManager.LoadScene("TopViewMap_1");
    }

    public void OpenHelpPanel()
    {
        HelpPanel.SetActive(true);
    }
    public void CloseHelpPanel()
    {
        HelpPanel.SetActive(false);
    }

}
