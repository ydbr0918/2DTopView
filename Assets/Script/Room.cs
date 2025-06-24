using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("이 방의 좌표")]
    public Vector2Int myRoomPos;

    [Header("시작 방 여부 (플레이어가 처음 스폰되는 방)")]
    public bool isStartRoom = false;

    [Header("문들")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("몬스터 스폰")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;

    // 클리어 여부 외부에서 읽기 전용
    public bool cleared { get; private set; } = false;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    /// <summary>
    /// 플레이어가 이 방으로 들어올 때 호출 (DoorPortal에서)
    /// </summary>
    public void OnPlayerEnter()
    {
        // 시작 방이거나 이미 클리어된 방이면 아무 일도 하지 않음
        if (isStartRoom || cleared) return;

        // 문 닫고
        SetDoorsActive(false);

        // 몬스터 스폰
        StartCoroutine(SpawnMonstersAfterDelay(1f));
    }

    private IEnumerator SpawnMonstersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
                Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f),
                0f
            );
            var m = Instantiate(monsterPrefab, pos, Quaternion.identity, transform);
            spawnedMonsters.Add(m);
        }

        // 몬스터 전멸 체크 시작
        StartCoroutine(CheckMonsters());
    }

    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            // 죽은(null) 몬스터 제거
            spawnedMonsters.RemoveAll(x => x == null);

            if (spawnedMonsters.Count == 0)
            {
                // 전부 잡혔으면 클리어
                cleared = true;
                SetDoorsActive(true);
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Up/Down/Left/Right 문을 한꺼번에 켜거나 끕니다.
    /// </summary>
    public void SetDoorsActive(bool on)
    {
        if (doorUp != null) doorUp.SetActive(on);
        if (doorDown != null) doorDown.SetActive(on);
        if (doorLeft != null) doorLeft.SetActive(on);
        if (doorRight != null) doorRight.SetActive(on);
    }
}
