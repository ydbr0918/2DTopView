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
        // ���� FindObjectOfType<UIManager> ���
        uiManager = FindObjectOfType<InGameUIManager>();
        if (uiManager == null)
            Debug.LogWarning("[Portal] InGameUIManager �� ã�� �� �����ϴ�!");
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
            Debug.LogWarning("[Portal] nextSceneName �� �������� �ʾҽ��ϴ�!");
    }
}
