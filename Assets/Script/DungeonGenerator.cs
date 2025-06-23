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

    [Header("�̴ϸ� ��Ʈ�ѷ� (Optional)")]
    public MiniMapController miniMapController;

    // ���� �� ��ǥ & ��ũ��Ʈ ���� ����
    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    private bool threeNeighborRoomExists = false;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // 1) �߾� �� ����
        Vector2Int center = Vector2Int.zero;
        roomPositions.Add(center);
        Room firstRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity)
                         .GetComponent<Room>();
        firstRoom.myRoomPos = center;
        firstRoom.isStartRoom = true;
        rooms.Add(center, firstRoom);

        // 2) ������ �� ����
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

        // 3) �� �ڵ� Ȱ��ȭ �� **�ߺ� Instantiate ���� ����**
        foreach (var pos in roomPositions)
        {
            GameObject roomGO = rooms[pos].gameObject;
            TrySetDoor(roomGO, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomGO, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomGO, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 4) �÷��̾� ���� (�߾� ��)
        Vector3 playerPos = new Vector3(center.x * roomSpacing, center.y * roomSpacing, 0f);
        Instantiate(playerPrefab, playerPos, Quaternion.identity);

        // 5) �̴ϸ� ���� (Optional)
        miniMapController?.SetupMiniMap(roomPositions, center);
    }

    // �� ������Ʈ �ϳ��� ����, ������ ���⿡ �̿��� ���� ���� ���� �� �ִ� ����
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

    // Room.cs �� DoorPortal.cs ���� ���� �޼����
    public Vector3 GetRoomWorldPos(Vector2Int p)
        => new Vector3(p.x * roomSpacing, p.y * roomSpacing, 0f);

    public Room GetRoomScript(Vector2Int p)
    {
        rooms.TryGetValue(p, out var r);
        return r;
    }

    // �� Ŭ���� �� ���� �ٽ� �� �ְ� ���� ��
    public void ActivateDoors(GameObject roomGO, Vector2Int roomPos)
    {
        TrySetDoor(roomGO, roomPos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
        TrySetDoor(roomGO, roomPos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
    }
}
