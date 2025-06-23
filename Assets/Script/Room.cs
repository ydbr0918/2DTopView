using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    public GameObject monsterPrefab;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;
    public int monsterCount = 3;

    [HideInInspector] public Vector2Int myRoomPos;
    [HideInInspector] public bool isStartRoom = false;

    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool cleared = false;
    private DungeonGenerator dungeon;

    public bool IsCleared => cleared;

    private void Awake()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
    }

    public void OnPlayerEnter()
    {
        if (isStartRoom || cleared) return;

        // 1) 문 닫기
        doorUp?.SetActive(false);
        doorDown?.SetActive(false);
        doorLeft?.SetActive(false);
        doorRight?.SetActive(false);

        // 2) 3초 뒤 몬스터 소환
        StartCoroutine(SpawnMonstersAfterDelay(3f));
    }

    IEnumerator SpawnMonstersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 randomPos = transform.position +
                new Vector3(
                    Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
                    Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f),
                    0
                );
            var monster = Instantiate(monsterPrefab, randomPos, Quaternion.identity, transform);
            spawnedMonsters.Add(monster);
        }

        StartCoroutine(CheckMonsters());
    }

    IEnumerator CheckMonsters()
    {
        while (true)
        {
            spawnedMonsters.RemoveAll(m => m == null);
            if (spawnedMonsters.Count == 0)
            {
                cleared = true;
                // 클리어된 방만 문을 다시 켭니다
                dungeon?.ActivateDoors(gameObject, myRoomPos);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
