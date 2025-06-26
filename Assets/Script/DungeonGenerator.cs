using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Vector2Int CurrentRoomPos { get; set; }

    [Header("방 프리팹 & 개수 & 간격")]
    public GameObject roomPrefab;
    public int roomCount = 7;
    public float roomSpacing = 14f;

    [Header("플레이어 프리팹")]
    public GameObject playerPrefab;

    [Header("끝 방 포탈 프리팹")]
    public GameObject portalPrefab;


    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
    private List<Vector2Int> roomPositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    private bool threeNeighborRoomExists = false;


    public List<Vector2Int> RoomPositions => roomPositions;

   

    public List<Vector2Int> ClearedRooms
        => roomPositions.Where(p => rooms[p].cleared).ToList();

    void Start()
    {

        if (SaveManager.HasSave)
        {
          
            SaveManager.LoadGame();
            var d = SaveManager.LoadedData;

            
            occupied.Clear();
            roomPositions.Clear();
            rooms.Clear();
            foreach (var sv in d.roomPositions)
            {
                Vector2Int coord = sv.ToVector2Int();
                bool isStart = coord == d.currentRoomPos.ToVector2Int();
                CreateRoomAt(coord, isStart);
            }

           
            foreach (var sv in d.clearedRooms)
            {
                Vector2Int coord = sv.ToVector2Int();
                var room = rooms[coord];
                room.cleared = true;
                room.RestoreOriginalDoors();
            }

         
            SetupAllDoors();

      
            Vector2Int cp = d.currentRoomPos.ToVector2Int();
            Instantiate(playerPrefab,
                        new Vector3(cp.x * roomSpacing, cp.y * roomSpacing, 0f),
                        Quaternion.identity);
        }
        else
        {
          
            GenerateDungeon();
        }
    }

    void GenerateDungeon()
    {
        occupied.Clear();
        roomPositions.Clear();
        rooms.Clear();

      
        Vector2Int start = Vector2Int.zero;
        CreateRoomAt(start, true);

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

      
        Instantiate(playerPrefab,
                    new Vector3(start.x * roomSpacing, start.y * roomSpacing, 0f),
                    Quaternion.identity);

        
        Vector2Int exitPos = roomPositions
            .OrderByDescending(p => Mathf.Abs(p.x - start.x) + Mathf.Abs(p.y - start.y))
            .First();

 
        var exitRoom = rooms[exitPos];
        exitRoom.isExitRoom = true;
        exitRoom.portalPrefab = portalPrefab;
    }

    private void SetupAllDoors()
    {
        foreach (var pos in roomPositions)
        {
            var roomComp = rooms[pos];
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.up, "Up Door", new Vector3(0, -2, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.down, "Down Door", new Vector3(0, 2, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.left, "Left Door", new Vector3(2, 0, 0));
            TrySetDoor(roomComp.gameObject, pos, Vector2Int.right, "Right Door", new Vector3(-2, 0, 0));
        }
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

        Vector2Int target = myPos + dir;
        bool hasNeighbor = occupied.Contains(target);

        // 문 켜/끄기
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
