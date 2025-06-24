using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    public string nextSceneName;

    private InGameUIManager uiManager;
    private bool playerInRange = false;

    private void Start()
    {
        // 기존 FindObjectOfType<UIManager> 대신
        uiManager = FindObjectOfType<InGameUIManager>();
        if (uiManager == null)
            Debug.LogWarning("[Portal] InGameUIManager 를 찾을 수 없습니다!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiManager.ShowMoveButton(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiManager.HideMoveButton();
        }
    }

    public void MoveToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("[Portal] nextSceneName 이 설정되지 않았습니다!");
    }
}
