using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HelpPanel;

    public WeaponSelector weaponSelector; // Inspector 에 드래그
    public SkillSelector skillSelector;  // Inspector 에 드래그

    public void SelectButtonAction()
    {
        SceneManager.LoadScene("SelectRoom");
    }

    public void GameStartButtonAction()
    {
        // 씬 전환 전에 무기·스킬 선택 정보를 확정
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
            Debug.LogWarning("저장된 데이터가 없습니다!");
            return;
        }

        SaveManager.LoadGame();
        SceneManager.LoadScene("TopViewMap_1");
    }
}
