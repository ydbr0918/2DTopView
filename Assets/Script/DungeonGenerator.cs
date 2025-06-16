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

    private bool threeNeighborRoomExists = false; // 3��¥�� �� ���� ����

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
            // 1. �̿��� 2�� �̸�(Ȥ�� 3�� �̸�, ���ǿ� ����)�� �游 �ĺ�
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

            // "�̿��� 3���� �Ǵ� ��"�� �� 1���� ���
            if (newPosNeighborCount >= 2)
            {
                if (!threeNeighborRoomExists && newPosNeighborCount == 2)
                {
                    threeNeighborRoomExists = true; // �������� 3��¥�� ���� �� ������!
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

        // �÷��̾�� ���� ù �濡 �� ���� ��ȯ!
        Vector2Int startRoom = roomPositions[0];
        Vector3 playerSpawnPos = new Vector3(startRoom.x * roomSpacing, startRoom.y * roomSpacing, 0);
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    }

    // ���� ��ġ�� �̿� ��(�����¿�) ���� ���ϱ�
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
