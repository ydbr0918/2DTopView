using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HelpPanel;

    public WeaponSelector weaponSelector; 
    public SkillSelector skillSelector;  

    public void SelectButtonAction()
    {
        SceneManager.LoadScene("SelectRoom");
    }

    public void GameStartButtonAction()
    {
       
        weaponSelector.ConfirmWeapon();
        skillSelector.ConfirmSkill();

        SaveManager.DeleteSave();

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

    public void OnContinue()
    {
        if (!SaveManager.HasSave)
        {
            Debug.LogWarning("����� �����Ͱ� �����ϴ�!");
            return;
        }

        SaveManager.LoadGame();
        SceneManager.LoadScene("TopViewMap_1");
    }
}
