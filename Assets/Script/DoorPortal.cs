using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    [HideInInspector] public Vector2Int myRoomPos;
    [HideInInspector] public Vector2Int targetRoomPos;
    [HideInInspector] public Vector3 entryOffset;

    private DungeonGenerator dungeon;
    private MiniMapController miniMapController;

    private void Start()
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

        // 2) 미니맵 업데이트 (Optional)
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 3) 새 방 로직
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // 4) 이 문(origin) 처리
        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        // 시작 방(isStartRoom)은 닫지 않고,
        // 아직 클리어되지 않은 방(origin)이면 문을 닫습니다.
        if (originRoom != null && !originRoom.isStartRoom && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
