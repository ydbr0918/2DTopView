using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("�̵� ��ư (Canvas > MoveButton)")]
    public Button moveButton;

    [Header("���ε� �˾� �ؽ�Ʈ")]
    public TextMeshProUGUI reloadText;

    private Portal currentPortal;

    private void Awake()
    {
        moveButton.gameObject.SetActive(false);
        reloadText.gameObject.SetActive(false);
        moveButton.onClick.AddListener(OnMoveButtonClicked);
    }

    // ������ ���� �� �ؽ�Ʈ ��� �ѱ�
    public void ShowReloadText()
    {
        StopAllCoroutines();       // Ȥ�� ���� �ڷ�ƾ�� ���� ������ �ߴ�
        reloadText.gameObject.SetActive(true);
    }

    // ������ �Ϸ� �� �ؽ�Ʈ ��� ����
    public void HideReloadText()
    {
        StopAllCoroutines();
        reloadText.gameObject.SetActive(false);
    }

    // ���� ���� �̵� ��ư ����...
    public void ShowMoveButton(Portal portal)
    {
        currentPortal = portal;
        moveButton.gameObject.SetActive(true);
    }
    public void HideMoveButton()
    {
        currentPortal = null;
        moveButton.gameObject.SetActive(false);
    }
    private void OnMoveButtonClicked()
    {
        currentPortal?.MoveToNextScene();
    }
}
