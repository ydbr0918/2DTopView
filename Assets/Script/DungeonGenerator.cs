using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("방 프리팹 & 개수 & 간격")]
    public GameObject roomPrefab;
    public int roomCount = 7;
    public float roomSpacing = 14f;

    [Header("플레이어 프리팹")]
    public GameObject playerPrefab;

    [Header("미니맵 컨트롤러 (Optional)")]
    public MiniMapController miniMapController;

    // 실제 방 좌표 & 스크립트 참조 보관
    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    private bool threeNeighborRoomExists = false;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // 1) 중앙 방 생성
        Vector2Int center = Vector2Int.zero;
        roomPositions.Add(center);
        Room firstRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity)
                         .GetComponent<Room>();
        firstRoom.myRoomPos = center;
        firstRoom.isStartRoom = true;
        rooms.Add(center, firstRoom);

        // 2) 나머지 방 생성
        int count = 1;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (count < roomCount)
        {
            var candidates = roomPositions
              .Where(p =>
                  GetNeighborCount(p) < 2 ||
                  (!threeNeighborRoomExists && GetNeighborCount(p) < 3)
              ).ToList();
            if (candidates.Count == 0) break;

            var basePos = candidates[Random.Range(0, candidates.Count)];
            var dir = directions[Random.Range(0, directions.Length)];
            var newPos = basePos + dir;
            if (roomPositions.Contains(newPos)) continue;

            int nc = GetNeighborCount(newPos);
            if (nc >= 2)
            {
                if (!threeNeighborRoomExists && nc == 2)
                    threeNeighborRoomExists = true;
                else
                    continue;
            }

            roomPositions.Add(newPos);
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing, newPos.y * roomSpacing, 0f);
            Room newRoom = Instantiate(roomPrefab, spawnPos, Quaternion.identity)
                           .GetComponent<Room>();
            newRoom.myRoomPos = newPos;
            rooms.Add(newPos, newRoom);

            count++;
        }

        // 3) 문 자동 활성화 → **중복 Instantiate 절대 금지**
        foreach (var pos in roomPositions)
        {
            GameObject roomGO = rooms[pos].gameObject;
            TrySetDoor(roomGO, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomGO, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 4) 플레이어 스폰 (중앙 방)
        Vector3 playerPos = new Vector3(center.x * roomSpacing, center.y * roomSpacing, 0f);
        Instantiate(playerPrefab, playerPos, Quaternion.identity);

        // 5) 미니맵 세팅 (Optional)
        miniMapController?.SetupMiniMap(roomPositions, center);
    }

    // 방 오브젝트 하나에 대해, 지정한 방향에 이웃이 있을 때만 문을 켜 주는 헬퍼
    void TrySetDoor(GameObject roomGO, Vector2Int myPos,
                   Vector2Int dir, string doorName, Vector3 entryOffset)
    {
        Vector2Int targetPos = myPos + dir;
        Transform doorTr = roomGO.transform.Find(doorName);
        if (doorTr == null) return;

        bool hasNeighbor = roomPositions.Contains(targetPos);
        doorTr.gameObject.SetActive(hasNeighbor);
        if (hasNeighbor)
        {
            var portal = doorTr.GetComponent<DoorPortal>();
            portal.myRoomPos = myPos;
            portal.targetRoomPos = targetPos;
            portal.entryOffset = entryOffset;
        }
    }

    int GetNeighborCount(Vector2Int p)
    {
        return new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right }
            .Count(d => roomPositions.Contains(p + d));
    }

    // Room.cs 및 DoorPortal.cs 에서 쓰는 메서드들
    public Vector3 GetRoomWorldPos(Vector2Int p)
        => new Vector3(p.x * roomSpacing, p.y * roomSpacing, 0f);

    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }

    // 방 클리어 후 문을 다시 켜 주고 싶을 때
    public void ActivateDoors(GameObject roomGO, Vector2Int roomPos)
    {
        TrySetDoor(roomGO, roomPos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
    }
}
