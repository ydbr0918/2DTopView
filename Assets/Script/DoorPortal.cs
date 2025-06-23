using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    public Vector2Int myRoomPos;       // 이 문이 속한 방 좌표
    public Vector2Int targetRoomPos;   // 이동할 방 좌표
    public Vector3 entryOffset;        // 타겟 방 입장 위치 오프셋

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
        Vector3 tp = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = tp;

        // 2) 미니맵 업데이트
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 3) 새 방 진입 로직 호출
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // 4) 이 문(=origin room 쪽 문)은, 
        //    아직 클리어되지 않은 방(=myRoomPos)에 속해 있으면 꺼주고,
        //    이미 클리어된 방이면 켜둡니다.
        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.IsCleared)
        {
            gameObject.SetActive(false);
        }
        // else: 이미 클리어된 방이니 문을 열어둡니다(켜둡니다).
    }
}
