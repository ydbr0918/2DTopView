using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("이 방의 좌표")]
    public Vector2Int myRoomPos;

    [Header("시작 방 여부")]
    public bool isStartRoom = false;

    [Header("문들")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("몬스터")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;

    // 원래 문 활성화 상태 저장용
    [HideInInspector] public bool origUp, origDown, origLeft, origRight;

    // 클리어 여부
    public bool cleared { get; private set; } = false;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    // 플레이어 입장 시 호출
    public void OnPlayerEnter()
    {
        if (isStartRoom || cleared) return;

        // 문 닫기
        SetDoorsActive(false);
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

        StartCoroutine(CheckMonsters());
    }

    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            spawnedMonsters.RemoveAll(x => x == null);
            if (spawnedMonsters.Count == 0)
            {
                cleared = true;
                // ▶ 원래 있던 문만 복원
                RestoreOriginalDoors();
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Up/Down/Left/Right 문을 한꺼번에 켜거나 끕니다.
    public void SetDoorsActive(bool on)
    {
        doorUp?.SetActive(on);
        doorDown?.SetActive(on);
        doorLeft?.SetActive(on);
        doorRight?.SetActive(on);
    }

    // ▶ 클리어 후 "원래 있던 문"만 켭니다.
    public void RestoreOriginalDoors()
    {
        if (doorUp != null) doorUp.SetActive(origUp);
        if (doorDown != null) doorDown.SetActive(origDown);
        if (doorLeft != null) doorLeft.SetActive(origLeft);
        if (doorRight != null) doorRight.SetActive(origRight);
    }
}
