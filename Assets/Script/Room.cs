using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("이 방의 좌표")]
    public Vector2Int myRoomPos;

    [Header("시작 방 여부")]
    public bool isStartRoom = false;

    [Header("끝 방 여부")]
    public bool isExitRoom = false;

    [Header("문들")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("몬스터 스폰 설정")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;

    [Header("포탈 프리팹 (끝 방 전용)")]
    public GameObject portalPrefab;

    // 클리어 여부 (외부에서만 읽기)
    public bool cleared { get;  set; } = false;

    // 문 복원을 위한 원래 상태 저장 필드
    [HideInInspector] public bool origUp, origDown, origLeft, origRight;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private void Awake()
    {
        // 던전 생성 시 DungeonGenerator에서 SetDoorsActive(false) 후
        // origX 값이 할당되므로 여긴 비워둡니다.
    }

    /// <summary>
    /// 플레이어가 이 방에 들어왔을 때 DungeonGenerator에서 호출
    /// </summary>
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
            // 죽은(null) 몬스터 제거
            spawnedMonsters.RemoveAll(x => x == null);

            if (spawnedMonsters.Count == 0)
            {
                cleared = true;

                // 원래 있던 문 상태만 복원
                RestoreOriginalDoors();

                // 끝 방이면 포탈 생성
                if (isExitRoom && portalPrefab != null)
                {
                    Instantiate(portalPrefab, transform.position, Quaternion.identity);
                }

                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Up/Down/Left/Right 문을 일괄 On/Off
    /// </summary>
    public void SetDoorsActive(bool on)
    {
        if (doorUp != null) doorUp.SetActive(on);
        if (doorDown != null) doorDown.SetActive(on);
        if (doorLeft != null) doorLeft.SetActive(on);
        if (doorRight != null) doorRight.SetActive(on);
    }

    /// <summary>
    /// 생성 시 DungeonGenerator에서 기록해 둔 origUp/origDown/... 값만 켜 줍니다.
    /// </summary>
    public void RestoreOriginalDoors()
    {
        if (doorUp != null) doorUp.SetActive(origUp);
        if (doorDown != null) doorDown.SetActive(origDown);
        if (doorLeft != null) doorLeft.SetActive(origLeft);
        if (doorRight != null) doorRight.SetActive(origRight);
    }
}
