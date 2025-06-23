// Room.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int myRoomPos;
    public GameObject doorUp, doorDown, doorLeft, doorRight;

    [Header("���� ���� ����")]
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
        // ���� �� ǥ��
        // (ù �� ���� ���� GenerateDungeon ���� isStartRoom = true �� �����ص� �����ϴ�)
        dungeon = FindObjectOfType<DungeonGenerator>();
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� �� DungeonGenerator ���� ȣ��
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

        // ���� ��ġ�� ���� ����
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
                // ���� ���� �������� �ٽ� �� Ȱ��ȭ
                dungeon?.ActivateDoors(gameObject, myRoomPos);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// ���ο��� ���� �� ON/OFF
    /// </summary>
    public void SetDoorsActive(bool on)
    {
        if (doorUp) doorUp.SetActive(on);
        if (doorDown) doorDown.SetActive(on);
        if (doorLeft) doorLeft.SetActive(on);
        if (doorRight) doorRight.SetActive(on);
    }
}
