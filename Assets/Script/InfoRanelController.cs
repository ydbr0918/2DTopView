using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;            // InfoPanel ������Ʈ
    [SerializeField] private TMP_Text titleText;          // TitleText
    [SerializeField] private TMP_Text descriptionText;    // DescriptionText
    [SerializeField] private Button closeButton;          // CloseButton

    private void Awake()
    {
        // ���� �� ������ �����
        panel.SetActive(false);
        // �ݱ� ��ư�� ������ �ɱ�
        closeButton.onClick.AddListener(Hide);
    }

    /// <summary>
    /// ������ ���� �ݴϴ�.
    /// </summary>
    public void Show(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
        panel.SetActive(true);
    }

    /// <summary>
    /// �г� �����
    /// </summary>
    public void Hide()
    {
        panel.SetActive(false);
    }
}
