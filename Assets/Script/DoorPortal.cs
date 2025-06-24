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

        // 1) �÷��̾� �ڷ���Ʈ
        Vector3 wp = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = wp;

        // 2) �̴ϸ� ������Ʈ (Optional)
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 3) �� �� ����
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // 4) �� ��(origin) ó��
        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        // ���� ��(isStartRoom)�� ���� �ʰ�,
        // ���� Ŭ������� ���� ��(origin)�̸� ���� �ݽ��ϴ�.
        if (originRoom != null && !originRoom.isStartRoom && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
