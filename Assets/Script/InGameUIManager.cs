using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("이동 버튼 (Canvas > MoveButton)")]
    public Button moveButton;

    private Portal currentPortal;

    private void Awake()
    {
        // 버튼 초기 숨김
        moveButton.gameObject.SetActive(false);
        moveButton.onClick.AddListener(OnMoveButtonClicked);
    }

    /// <summary>
    /// Portal 에서 플레이어 진입 시 호출
    /// </summary>
    public void ShowMoveButton(Portal portal)
    {
        currentPortal = portal;
        moveButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Portal 에서 플레이어 퇴장 시 호출
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
