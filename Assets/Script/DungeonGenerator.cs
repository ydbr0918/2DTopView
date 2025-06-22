using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 7;
    public float roomSpacing = 14f;
    public GameObject playerPrefab;

    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    private bool threeNeighborRoomExists = false;
    public MiniMapController miniMapController;
    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int center = Vector2Int.zero;
        roomPositions.Add(center);
        Room firstRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        rooms.Add(center, firstRoom);

        int count = 1;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (count < roomCount)
        {
            List<Vector2Int> candidateRooms = roomPositions
                .Where(pos =>
                    GetNeighborCount(pos) < 2 ||
                    (!threeNeighborRoomExists && GetNeighborCount(pos) < 3)
                )
                .ToList();

            if (candidateRooms.Count == 0)
                break;

            Vector2Int basePos = candidateRooms[Random.Range(0, candidateRooms.Count)];
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            Vector2Int newPos = basePos + dir;

            if (roomPositions.Contains(newPos)) continue;

            int newPosNeighborCount = GetNeighborCount(newPos);

            if (newPosNeighborCount >= 2)
            {
                if (!threeNeighborRoomExists && newPosNeighborCount == 2)
                {
                    threeNeighborRoomExists = true;
                }
                else
                {
                    continue;
                }
            }

            roomPositions.Add(newPos);
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing, newPos.y * roomSpacing, 0);
            Room newRoom = Instantiate(roomPrefab, spawnPos, Quaternion.identity).GetComponent<Room>();
            rooms.Add(newPos, newRoom);

            count++;
        }

        // 문 자동 활성화
        foreach (Vector2Int roomPos in roomPositions)
        {
            GameObject roomGO = rooms[roomPos].gameObject; // 이미 생성된 방 객체
                                                           // 각 문(Up, Down, Left, Right)을 찾아서 이웃이 있을 때만 활성화하고 연결
            TrySetDoor(roomGO, roomPos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomGO, roomPos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomGO, roomPos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomGO, roomPos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }

        // 플레이어는 가장 첫 방에 한 번만 소환!
        Vector2Int startRoom = roomPositions[0];
        Vector3 playerSpawnPos = new Vector3(startRoom.x * roomSpacing, startRoom.y * roomSpacing, 0);
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);

        miniMapController.SetupMiniMap(roomPositions, startRoom);
    }

    void TrySetDoor(GameObject roomGO, Vector2Int myPos, Vector2Int dir, string doorName, Vector3 entryOffset)
    {
        Vector2Int targetPos = myPos + dir;
        Transform doorTr = roomGO.transform.Find(doorName);

        if (doorTr != null)
        {
            bool hasNeighbor = roomPositions.Contains(targetPos);
            doorTr.gameObject.SetActive(hasNeighbor);

            if (hasNeighbor)
            {
                DoorPortal portal = doorTr.GetComponent<DoorPortal>();
                if (portal != null)
                {
                    portal.myRoomPos = myPos;
                    portal.targetRoomPos = targetPos;
                    portal.entryOffset = entryOffset;
                }
            }
        }
    }

    int GetNeighborCount(Vector2Int pos)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        int count = 0;
        foreach (var dir in directions)
        {
            if (roomPositions.Contains(pos + dir)) count++;
        }
        return count;
    }

    public Vector3 GetRoomWorldPos(Vector2Int roomPos)
    {
        return new Vector3(roomPos.x * roomSpacing, roomPos.y * roomSpacing, 0);
    }

    void ActivateDoors(GameObject roomObj, Vector2Int roomPos)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        string[] doorNames = { "Up Door", "Down Door", "Left Door", "Right Door" };
        Vector3[] entryOffsets = {
        new Vector3(0, -6, 0), // 위문 -> 아래에서 입장
        new Vector3(0, 6, 0),  // 아래문 -> 위에서 입장
        new Vector3(6, 0, 0),  // 왼쪽문 -> 오른쪽에서 입장
        new Vector3(-6, 0, 0), // 오른쪽문 -> 왼쪽에서 입장
    };

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int neighbor = roomPos + dirs[i];
            Transform door = roomObj.transform.Find(doorNames[i]);
            if (door != null)
            {
                bool hasNeighbor = roomPositions.Contains(neighbor);
                door.gameObject.SetActive(hasNeighbor);

                if (hasNeighbor)
                {
                    DoorPortal portal = door.GetComponent<DoorPortal>();
                    if (portal != null)
                    {
                        portal.myRoomPos = roomPos;
                        portal.targetRoomPos = neighbor;
                        portal.entryOffset = entryOffsets[i];
                    }
                }
            }
        }
    }

    public Room GetRoomScript(Vector2Int roomPos)
    {
        if (rooms.TryGetValue(roomPos, out Room room))
            return room;
        return null;
    }

}

