using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;     
    public int roomCount = 7;         
    public float roomSpacing = 14f;   
    public GameObject playerPrefab;


    private List<Vector2Int> roomPositions = new List<Vector2Int>();

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
        while (count < roomCount)
        {
            // 이미 생성된 방 중 하나를 랜덤 선택
            Vector2Int basePos = roomPositions[Random.Range(0, roomPositions.Count)];
            // 랜덤한 방향
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            Vector2Int newPos = basePos + dir;

            // 이미 그 자리에 방이 있으면 스킵(중복 방지)
            if (roomPositions.Contains(newPos)) continue;

            // 새 방 생성
            roomPositions.Add(newPos);
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing, newPos.y * roomSpacing, 0);
            Instantiate(roomPrefab, spawnPos, Quaternion.identity);

            count++;

        }
        // 예: roomPositions[0]이 첫 번째 방의 좌표라고 할 때
        Vector2Int startRoom = roomPositions[0];
        Vector3 playerSpawnPos = new Vector3(startRoom.x * roomSpacing, startRoom.y * roomSpacing, 0);
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    }
}
