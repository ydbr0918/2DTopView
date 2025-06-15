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
            // �̹� ������ �� �� �ϳ��� ���� ����
            Vector2Int basePos = roomPositions[Random.Range(0, roomPositions.Count)];
            // ������ ����
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            Vector2Int newPos = basePos + dir;

            // �̹� �� �ڸ��� ���� ������ ��ŵ(�ߺ� ����)
            if (roomPositions.Contains(newPos)) continue;

            // �� �� ����
            roomPositions.Add(newPos);
            Vector3 spawnPos = new Vector3(newPos.x * roomSpacing, newPos.y * roomSpacing, 0);
            Instantiate(roomPrefab, spawnPos, Quaternion.identity);

            count++;

        }
        // ��: roomPositions[0]�� ù ��° ���� ��ǥ��� �� ��
        Vector2Int startRoom = roomPositions[0];
        Vector3 playerSpawnPos = new Vector3(startRoom.x * roomSpacing, startRoom.y * roomSpacing, 0);
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    }
}
