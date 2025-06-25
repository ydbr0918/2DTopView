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

        // �ڷ���Ʈ
        var dest = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = dest;

        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // �� ����
        var targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // origin �� �ݱ� (���� �浵, Ŭ����� �浵 ���� ����)
        var originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.isStartRoom && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
