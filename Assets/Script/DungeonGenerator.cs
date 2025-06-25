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

    [Header("끝 방 포탈 프리팹")]
    public GameObject portalPrefab;

    // 생성된 방 좌표 & 스크립트 참조 보관
    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    private bool threeNeighborRoomExists = false;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        occupied.Clear();
        roomPositions.Clear();
        rooms.Clear();

        // 1) 시작 방 (0,0)
        Vector2Int start = Vector2Int.zero;
        CreateRoomAt(start, true);

        // 2) 나머지 방 생성
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        int created = 1;
        while (created < roomCount)
        {
            var candidates = roomPositions
                .Where(p => GetNeighborCount(p) < 2 ||
                            (!threeNeighborRoomExists && GetNeighborCount(p) < 3))
                .ToList();
            if (candidates.Count == 0) break;

            var basePos = candidates[Random.Range(0, candidates.Count)];
            var dir = dirs[Random.Range(0, dirs.Length)];
            var newPos = basePos + dir;

            if (occupied.Contains(newPos))
                continue;

            int nc = GetNeighborCount(newPos);
            if (nc >= 2)
            {
                if (!threeNeighborRoomExists && nc == 2)
                    threeNeighborRoomExists = true;
                else
                    continue;
            }

            CreateRoomAt(newPos, false);
            created++;
        }

        // 3) 각 방에 문 세팅
        // 예시: DungeonGenerator.cs 의 문 세팅 부분
        foreach (var pos in roomPositions)
        {
            var roomComp = rooms[pos];
            bool hasUp = occupied.Contains(pos + Vector2Int.up);
            bool hasDown = occupied.Contains(pos + Vector2Int.down);
            bool hasLeft = occupied.Contains(pos + Vector2Int.left);
            bool hasRight = occupied.Contains(pos + Vector2Int.right);

            roomComp.origUp = hasUp;
            roomComp.origDown = hasDown;
            roomComp.origLeft = hasLeft;
            roomComp.origRight = hasRight;

            TrySetDoor(roomComp.gameObject, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 4) 플레이어 스폰
        Instantiate(playerPrefab,
                    new Vector3(start.x * roomSpacing, start.y * roomSpacing, 0f),
                    Quaternion.identity);

        // 5) “끝 방” 결정: 시작방에서 맨해튼 거리가 가장 먼 방
        Vector2Int exitPos = roomPositions
            .OrderByDescending(p => Mathf.Abs(p.x - start.x) + Mathf.Abs(p.y - start.y))
            .First();

        // 그 방에 isExitRoom = true, portalPrefab 할당
        var exitRoom = rooms[exitPos];
        exitRoom.isExitRoom = true;
        exitRoom.portalPrefab = portalPrefab;
    }

    private void CreateRoomAt(Vector2Int coord, bool isStart)
    {
        Vector3 wp = new Vector3(coord.x * roomSpacing, coord.y * roomSpacing, 0f);
        var room = Instantiate(roomPrefab, wp, Quaternion.identity)
                   .GetComponent<Room>();

        room.myRoomPos = coord;
        room.isStartRoom = isStart;
        room.SetDoorsActive(false);

        occupied.Add(coord);
        roomPositions.Add(coord);
        rooms[coord] = room;
    }

    private int GetNeighborCount(Vector2Int p)
    {
        return new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right }
            .Count(d => occupied.Contains(p + d));
    }

    private void TrySetDoor(
        GameObject roomGO,
        Vector2Int myPos,
        Vector2Int dir,
        string doorName,
        Vector3 entryOffset
    )
    {
        var tr = roomGO.transform.Find(doorName);
        if (tr == null) return;

        bool hasNeighbor = occupied.Contains(myPos + dir);
        tr.gameObject.SetActive(hasNeighbor);
        if (hasNeighbor)
        {
            var portal = tr.GetComponent<DoorPortal>();
            portal.myRoomPos = myPos;
            portal.targetRoomPos = myPos + dir;
            portal.entryOffset = entryOffset;
        }
    }

    // DoorPortal → 플레이어 텔레포트용
    public Vector3 GetRoomWorldPos(Vector2Int p)
        => new Vector3(p.x * roomSpacing, p.y * roomSpacing, 0f);

    // DoorPortal → Room.OnPlayerEnter 호출용
    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }
}
