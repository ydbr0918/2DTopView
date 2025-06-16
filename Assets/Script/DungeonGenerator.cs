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

    private bool threeNeighborRoomExists = false; // 3방짜리 방 생성 여부

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int center = Vector2Int.zero;
        roomPositions.Add(center);
        Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        int count = 1;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (count < roomCount)
        {
            // 1. 이웃이 2개 미만(혹은 3개 미만, 조건에 따라)인 방만 후보
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

            // "이웃이 3개가 되는 방"은 단 1개만 허용
            if (newPosNeighborCount >= 2)
            {
                if (!threeNeighborRoomExists && newPosNeighborCount == 2)
                {
                    threeNeighborRoomExists = true; // 이제부터 3방짜리 방은 또 못만듦!
                }
                else
                {
                    continue;
                }
            }

            roomPositions.Add(newPos);
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing, newPos.y * roomSpacing, 0);
            Instantiate(roomPrefab, spawnPos, Quaternion.identity);

            count++;
        }

        // 플레이어는 가장 첫 방에 한 번만 소환!
        Vector2Int startRoom = roomPositions[0];
        Vector3 playerSpawnPos = new Vector3(startRoom.x * roomSpacing, startRoom.y * roomSpacing, 0);
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    }

    // 현재 위치의 이웃 방(상하좌우) 개수 구하기
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
}
