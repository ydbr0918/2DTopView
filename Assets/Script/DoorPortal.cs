using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    public Vector2Int myRoomPos;
    public Vector2Int targetRoomPos;
    public Vector3 entryOffset;

    private DungeonGenerator dungeon;
    private MiniMapController miniMapController;

    private void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        miniMapController = FindObjectOfType<MiniMapController>(); // ���⼭ ã��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dungeon != null)
        {
            Vector3 targetWorldPos = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
            other.transform.position = targetWorldPos;

            // **�̴ϸʿ� �� ��ġ ����**
            if (miniMapController != null)
            {
                miniMapController.UpdatePlayerIcon(targetRoomPos);
            }

            Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
            if (targetRoom != null)
            {
                targetRoom.OnPlayerEnter();
            }

            // �� ���� ��� ������� (�÷��̾� �̵� �Ϸ� ��)
            gameObject.SetActive(false);
        }
    }
}
