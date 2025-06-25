using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    [HideInInspector] public Vector2Int myRoomPos;
    [HideInInspector] public Vector2Int targetRoomPos;
    [HideInInspector] public Vector3 entryOffset;

    private DungeonGenerator dungeon;
    private MiniMapController miniMapController;

    void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        miniMapController = FindObjectOfType<MiniMapController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || dungeon == null) return;

        // 1) 플레이어 텔레포트
        Vector3 wp = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = wp;

        // → 이 줄을 바로 아래에 추가하세요!
        dungeon.CurrentRoomPos = targetRoomPos;

        // 2) 미니맵 업데이트
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 3) 새 방 진입 로직 호출
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // 4) 이전(origin) 방 문 처리…
        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
