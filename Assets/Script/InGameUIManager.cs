using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("�̵� ��ư (Canvas > MoveButton)")]
    public Button moveButton;

    private Portal currentPortal;

    private void Awake()
    {
        // ��ư �ʱ� ����
        moveButton.gameObject.SetActive(false);
        moveButton.onClick.AddListener(OnMoveButtonClicked);
    }

    /// <summary>
    /// Portal ���� �÷��̾� ���� �� ȣ��
    /// </summary>
    public void ShowMoveButton(Portal portal)
    {
        currentPortal = portal;
        moveButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Portal ���� �÷��̾� ���� �� ȣ��
    /// </summary>
    public void HideMoveButton()
    {
        currentPortal = null;
        moveButton.gameObject.SetActive(false);
    }

    private void OnMoveButtonClicked()
    {
        if (currentPortal != null)
            currentPortal.MoveToNextScene();
    }
}
