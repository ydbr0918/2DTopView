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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || dungeon == null) return;

        // 텔레포트
        var dest = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = dest;

        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 방 로직
        var targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // origin 문 닫기 (시작 방도, 클리어된 방도 닫지 않음)
        var originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.isStartRoom && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
