using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public GameObject roomIconPrefab;         // 미니맵에 표시할 방 아이콘 프리팹
    public Transform miniMapPanel;            // 미니맵 패널 (UI 상에서 방 부모)
    public GameObject playerIcon;             // 플레이어 위치 아이콘 (미니맵 위)

    public float iconSpacing = 30f;           // 아이콘 간격 (픽셀 단위)
    private Dictionary<Vector2Int, GameObject> iconDict = new Dictionary<Vector2Int, GameObject>();

    private Vector2Int playerRoomPos;

    // 미니맵 방 생성 (던전 생성 후 호출)

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

    // 플레이어가 방을 옮길 때마다 호출
    public void UpdatePlayerIcon(Vector2Int playerRoom)
    {
        playerRoomPos = playerRoom;
        if (iconDict.TryGetValue(playerRoom, out GameObject icon))
        {
            playerIcon.GetComponent<RectTransform>().anchoredPosition = icon.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
