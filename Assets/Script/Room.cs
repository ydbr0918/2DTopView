using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("�� ���� ��ǥ")]
    public Vector2Int myRoomPos;

    [Header("���� �� ����")]
    public bool isStartRoom = false;

    [Header("�� �� ����")]
    public bool isExitRoom = false;

    [Header("����")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("���� ���� ����")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;

    [Header("��Ż ������ (�� �� ����)")]
    public GameObject portalPrefab;

    public bool cleared { get;  set; } = false;


    [HideInInspector] public bool origUp, origDown, origLeft, origRight;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private void Awake()
    {
      
    }

  
    public void OnPlayerEnter()
    {
        if (isStartRoom || cleared) return;

     
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
            // �������� ����
            spawnedMonsters.RemoveAll(x => x == null);

            if (spawnedMonsters.Count == 0)
            {
                cleared = true;

             
                RestoreOriginalDoors();

                if (isExitRoom && portalPrefab != null)
                {
                    Instantiate(portalPrefab, transform.position, Quaternion.identity);
                }

                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

  
    public void SetDoorsActive(bool on)
    {
        if (doorUp != null) doorUp.SetActive(on);
        if (doorDown != null) doorDown.SetActive(on);
        if (doorLeft != null) doorLeft.SetActive(on);
        if (doorRight != null) doorRight.SetActive(on);
    }

    public void RestoreOriginalDoors()
    {
        if (doorUp != null) doorUp.SetActive(origUp);
        if (doorDown != null) doorDown.SetActive(origDown);
        if (doorLeft != null) doorLeft.SetActive(origLeft);
        if (doorRight != null) doorRight.SetActive(origRight);
    }
}
