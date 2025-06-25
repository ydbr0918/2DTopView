using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("이동 버튼 (Canvas > MoveButton)")]
    public Button moveButton;

    [Header("리로드 팝업 텍스트")]
    public TextMeshProUGUI reloadText;

    private Portal currentPortal;

    private void Awake()
    {
        moveButton.gameObject.SetActive(false);
        reloadText.gameObject.SetActive(false);
        moveButton.onClick.AddListener(OnMoveButtonClicked);
    }

    // 재장전 시작 시 텍스트 즉시 켜기
    public void ShowReloadText()
    {
        StopAllCoroutines();       // 혹시 이전 코루틴이 돌고 있으면 중단
        reloadText.gameObject.SetActive(true);
    }

    // 재장전 완료 시 텍스트 즉시 끄기
    public void HideReloadText()
    {
        StopAllCoroutines();
        reloadText.gameObject.SetActive(false);
    }

    // 이하 기존 이동 버튼 로직...
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
