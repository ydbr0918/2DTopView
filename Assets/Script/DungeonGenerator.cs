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

    // 이미 생성된 좌표 체크용
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
        // 초기화
        occupied.Clear();
        roomPositions.Clear();
        rooms.Clear();

        // 1) 중앙 방 생성 (시작 방)
        Vector2Int center = Vector2Int.zero;
        CreateRoomAt(center, true);

        // 2) 나머지 방 생성
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        int created = 1;
        while (created < roomCount)
        {
            // 이웃 <2 또는 (3-이웃 방 아직 없고 이웃<3) 인 방들만 후보
            var candidates = roomPositions
                .Where(p =>
                    GetNeighborCount(p) < 2 ||
                    (!threeNeighborRoomExists && GetNeighborCount(p) < 3)
                )
                .ToList();

            if (candidates.Count == 0) break;

            var basePos = candidates[Random.Range(0, candidates.Count)];
            var dir = dirs[Random.Range(0, dirs.Length)];
            var newPos = basePos + dir;

            // ← 이미 방이 있던 자리면 SKIP
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

        // 3) 모든 방에 대해 문 세팅 (이웃 여부 + 시작 방 보정)
        foreach (var pos in roomPositions)
        {
            var roomGO = rooms[pos].gameObject;
            TrySetDoor(roomGO, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomGO, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 4) 플레이어 스폰 (시작 방)
        Instantiate(playerPrefab,
                    new Vector3(center.x * roomSpacing, center.y * roomSpacing, 0f),
                    Quaternion.identity);
    }

    /// <summary>
    /// coord 위치에 방을 만들고, 각종 컬렉션에 추가합니다.
    /// isStart == true 이면 '시작 방' 플래그가 켜집니다.
    /// </summary>
    private void CreateRoomAt(Vector2Int coord, bool isStart)
    {
        Vector3 wp = new Vector3(coord.x * roomSpacing, coord.y * roomSpacing, 0f);
        var room = Instantiate(roomPrefab, wp, Quaternion.identity)
                   .GetComponent<Room>();

        room.myRoomPos = coord;
        room.isStartRoom = isStart;
        // 처음엔 문을 모두 닫아둡니다
        room.SetDoorsActive(false);

        occupied.Add(coord);
        roomPositions.Add(coord);
        rooms[coord] = room;
    }

    /// <summary>
    /// p의 상하좌우에 이미 방이 몇 개 있는지 셉니다.
    /// </summary>
    private int GetNeighborCount(Vector2Int p)
    {
        return new[]
        {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        }.Count(d => occupied.Contains(p + d));
    }

    /// <summary>
    /// 한 방(roomGO)의 myPos에서 dir 방향에 neighbor가 있으면 그 문만 켜 주고,
    /// 시작 방(isStartRoom)이면 무조건 켜 줍니다.
    /// </summary>
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

        Vector2Int target = myPos + dir;
        bool hasNeighbor = occupied.Contains(target);

        // 시작 방이면 문 무조건 켜 주기
        var roomComp = rooms[myPos];
        bool active = hasNeighbor || roomComp.isStartRoom;

        tr.gameObject.SetActive(active);

        if (active && hasNeighbor)
        {
            var portal = tr.GetComponent<DoorPortal>();
            portal.myRoomPos = myPos;
            portal.targetRoomPos = target;
            portal.entryOffset = entryOffset;
        }
    }

    /// <summary>
    /// DoorPortal에서 플레이어를 옮길 때 씁니다.
    /// </summary>
    public Vector3 GetRoomWorldPos(Vector2Int p)
        => new Vector3(p.x * roomSpacing, p.y * roomSpacing, 0f);

    /// <summary>
    /// DoorPortal에서 Room.OnPlayerEnter() 호출할 때 씁니다.
    /// </summary>
    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }
}
