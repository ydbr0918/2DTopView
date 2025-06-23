// DungeonGenerator.cs
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

    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    private bool threeNeighborRoomExists = false;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // 1) 중앙 방
        Vector2Int center = Vector2Int.zero;
        roomPositions.Add(center);
        Room firstRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity)
                         .GetComponent<Room>();
        firstRoom.myRoomPos = center;
        rooms.Add(center, firstRoom);

        // 2) 나머지 방
        int count = 1;
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (count < roomCount)
        {
            // 후보 방 필터
            var candidates = roomPositions
                .Where(p => GetNeighborCount(p) < 2 ||
                             (!threeNeighborRoomExists && GetNeighborCount(p) < 3))
                .ToList();
            if (candidates.Count == 0) break;

            var basePos = candidates[Random.Range(0, candidates.Count)];
            var dir = dirs[Random.Range(0, dirs.Length)];
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
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing,
                                           newPos.y * roomSpacing,
                                           0f);
            Room newRoom = Instantiate(roomPrefab, spawnPos, Quaternion.identity)
                           .GetComponent<Room>();
            newRoom.myRoomPos = newPos;
            rooms.Add(newPos, newRoom);

            count++;
        }

        // 3) 문 자동 활성화
        foreach (var pos in roomPositions)
        {
            var roomGO = rooms[pos].gameObject;
            TrySetDoor(roomGO, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomGO, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 4) 플레이어 스폰 (첫 방)
        Vector2Int start = roomPositions[0];
        Vector3 wp = new Vector3(start.x * roomSpacing,
                                 start.y * roomSpacing,
                                 0f);
        Instantiate(playerPrefab, wp, Quaternion.identity);

        // 5) 미니맵 세팅 (Optional)
        miniMapController?.SetupMiniMap(roomPositions, start);
    }

    // private: 방 생성 시만 쓰입니다
    void TrySetDoor(GameObject roomGO, Vector2Int myPos,
                    Vector2Int dir, string doorName, Vector3 offset)
    {
        var target = myPos + dir;
        var doorTr = roomGO.transform.Find(doorName);
        if (doorTr == null) return;

        bool has = roomPositions.Contains(target);
        doorTr.gameObject.SetActive(has);
        if (has)
        {
            var portal = doorTr.GetComponent<DoorPortal>();
            if (portal != null)
            {
                portal.myRoomPos = myPos;
                portal.targetRoomPos = target;
                portal.entryOffset = offset;
            }
        }
    }

    // public: Room 스크립트에서 몬스터 전멸 후 호출
    public void ActivateDoors(GameObject roomGO, Vector2Int roomPos)
    {
        TrySetDoor(roomGO, roomPos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
    }

    int GetNeighborCount(Vector2Int p)
    {
        Vector2Int[] ds = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        int c = 0;
        foreach (var d in ds)
            if (roomPositions.Contains(p + d)) c++;
        return c;
    }

    // Room.cs 및 DoorPortal.cs 에서 씁니다
    public Vector3 GetRoomWorldPos(Vector2Int p)
    {
        return new Vector3(p.x * roomSpacing,
                           p.y * roomSpacing,
                           0f);
    }

    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }
}
