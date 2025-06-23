// Room.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int myRoomPos;
    public GameObject doorUp, doorDown, doorLeft, doorRight;

    [Header("몬스터 스폰 설정")]
    public GameObject monsterPrefab;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;
    public int monsterCount = 3;

    private bool isStartRoom = false;
    private bool cleared = false;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private DungeonGenerator dungeon;

    void Awake()
    {
        // 시작 방 표시
        // (첫 방 생성 직후 GenerateDungeon 에서 isStartRoom = true 로 세팅해도 좋습니다)
        dungeon = FindObjectOfType<DungeonGenerator>();
    }

    /// <summary>
    /// 플레이어가 문을 통해 들어올 때 DungeonGenerator 에서 호출
    /// </summary>
    public void OnPlayerEnter()
    {
        if (isStartRoom || cleared) return;
        SetDoorsActive(false);
        StartCoroutine(SpawnMonstersAfterDelay(3f));
    }

    IEnumerator SpawnMonstersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 랜덤 위치에 몬스터 스폰
        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 rnd = transform.position
                        + new Vector3(
                            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
                            Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f),
                            0f
                          );
            var m = Instantiate(monsterPrefab, rnd, Quaternion.identity, transform);
            spawnedMonsters.Add(m);
        }

        StartCoroutine(CheckMonsters());
    }

    IEnumerator CheckMonsters()
    {
        while (true)
        {
            spawnedMonsters.RemoveAll(x => x == null);
            if (spawnedMonsters.Count == 0)
            {
                cleared = true;
                // 몬스터 전부 잡혔으니 다시 문 활성화
                dungeon?.ActivateDoors(gameObject, myRoomPos);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 내부에서 쓰는 문 ON/OFF
    /// </summary>
    public void SetDoorsActive(bool on)
    {
        if (doorUp) doorUp.SetActive(on);
        if (doorDown) doorDown.SetActive(on);
        if (doorLeft) doorLeft.SetActive(on);
        if (doorRight) doorRight.SetActive(on);
    }
}
