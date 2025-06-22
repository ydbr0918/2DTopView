using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public GameObject roomIconPrefab;         // �̴ϸʿ� ǥ���� �� ������ ������
    public Transform miniMapPanel;            // �̴ϸ� �г� (UI �󿡼� �� �θ�)
    public GameObject playerIcon;             // �÷��̾� ��ġ ������ (�̴ϸ� ��)

    public float iconSpacing = 30f;           // ������ ���� (�ȼ� ����)
    private Dictionary<Vector2Int, GameObject> iconDict = new Dictionary<Vector2Int, GameObject>();

    private Vector2Int playerRoomPos;

    // �̴ϸ� �� ���� (���� ���� �� ȣ��)

    public void SetupMiniMap(List<Vector2Int> roomPositions, Vector2Int playerStartRoom)
    {
        foreach (var pos in roomPositions)
        {
            GameObject icon = Instantiate(roomIconPrefab, miniMapPanel);
            icon.GetComponent<RectTransform>().anchoredPosition = (Vector2)pos * iconSpacing;
            iconDict[pos] = icon;
        }

        playerRoomPos = playerStartRoom;
        UpdatePlayerIcon(playerRoomPos);
    }

    // �÷��̾ ���� �ű� ������ ȣ��
    public void UpdatePlayerIcon(Vector2Int playerRoom)
    {
        playerRoomPos = playerRoom;
        if (iconDict.TryGetValue(playerRoom, out GameObject icon))
        {
            playerIcon.GetComponent<RectTransform>().anchoredPosition = icon.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
