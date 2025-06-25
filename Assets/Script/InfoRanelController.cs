using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;            // InfoPanel 오브젝트
    [SerializeField] private TMP_Text titleText;          // TitleText
    [SerializeField] private TMP_Text descriptionText;    // DescriptionText
    [SerializeField] private Button closeButton;          // CloseButton

    private void Awake()
    {
        // 시작 시 무조건 숨기고
        panel.SetActive(false);
        // 닫기 버튼에 리스너 걸기
        closeButton.onClick.AddListener(Hide);
    }

    /// <summary>
    /// 정보를 보여 줍니다.
    /// </summary>
    public void Show(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
        panel.SetActive(true);
    }

    /// <summary>
    /// 패널 숨기기
    /// </summary>
    public void Hide()
    {
        panel.SetActive(false);
    }
}
