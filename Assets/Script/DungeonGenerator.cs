using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("�� ������ & ���� & ����")]
    public GameObject roomPrefab;
    public int roomCount = 7;
    public float roomSpacing = 14f;

    [Header("�÷��̾� ������")]
    public GameObject playerPrefab;

    // �̹� ������ ��ǥ üũ��
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

        // 1) ���� ��
        Vector2Int center = Vector2Int.zero;
        CreateRoomAt(center, true);

        // 2) ������ ��
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

        // 3) �� ���� (���� �̿� ���θ� üũ)
        foreach (var pos in roomPositions)
        {
            var roomGO = rooms[pos].gameObject;
            var roomComp = rooms[pos];

            // ����
            bool hasUp = occupied.Contains(pos + Vector2Int.up);
            TrySetDoor(roomGO, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            roomComp.origUp = hasUp;

            // �Ʒ���
            bool hasDown = occupied.Contains(pos + Vector2Int.down);
            TrySetDoor(roomGO, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            roomComp.origDown = hasDown;

            // ����
            bool hasLeft = occupied.Contains(pos + Vector2Int.left);
            TrySetDoor(roomGO, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            roomComp.origLeft = hasLeft;

            // ������
            bool hasRight = occupied.Contains(pos + Vector2Int.right);
            TrySetDoor(roomGO, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
            roomComp.origRight = hasRight;
        }

        // 4) �÷��̾� ����
        Instantiate(playerPrefab,
                    new Vector3(center.x * roomSpacing, center.y * roomSpacing, 0f),
                    Quaternion.identity);
    }

    private void CreateRoomAt(Vector2Int coord, bool isStart)
    {
        var worldPos = new Vector3(coord.x * roomSpacing, coord.y * roomSpacing, 0f);
        var room = Instantiate(roomPrefab, worldPos, Quaternion.identity)
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

        Vector2Int target = myPos + dir;
        bool hasNeighbor = occupied.Contains(target);

        // **����** hasNeighbor �� üũ (���� �浵 ���� ����)
        tr.gameObject.SetActive(hasNeighbor);

        if (hasNeighbor)
        {
            var portal = tr.GetComponent<DoorPortal>();
            portal.myRoomPos = myPos;
            portal.targetRoomPos = target;
            portal.entryOffset = entryOffset;
        }
    }

    public Vector3 GetRoomWorldPos(Vector2Int p)
        => new Vector3(p.x * roomSpacing, p.y * roomSpacing, 0f);

    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }
}
