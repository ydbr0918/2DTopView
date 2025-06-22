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
        miniMapController = FindObjectOfType<MiniMapController>(); // 여기서 찾기
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dungeon != null)
        {
            Vector3 targetWorldPos = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
            other.transform.position = targetWorldPos;

            // **미니맵에 방 위치 갱신**
            if (miniMapController != null)
            {
                miniMapController.UpdatePlayerIcon(targetRoomPos);
            }

            Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
            if (targetRoom != null)
            {
                targetRoom.OnPlayerEnter();
            }

            // 이 문은 잠시 사라지게 (플레이어 이동 완료 후)
            gameObject.SetActive(false);
        }
    }
}
