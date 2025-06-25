using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HelpPanel;

    public WeaponSelector weaponSelector; // Inspector �� �巡��
    public SkillSelector skillSelector;  // Inspector �� �巡��

    public void SelectButtonAction()
    {
        SceneManager.LoadScene("SelectRoom");
    }

    public void GameStartButtonAction()
    {
        // �� ��ȯ ���� ���⡤��ų ���� ������ Ȯ��
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
