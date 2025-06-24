using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("�� ���� ��ǥ")]
    public Vector2Int myRoomPos;

    [Header("���� �� ���� (�÷��̾ ó�� �����Ǵ� ��)")]
    public bool isStartRoom = false;

    [Header("����")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("���� ����")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnAreaWidth = 6f;
    public float spawnAreaHeight = 6f;

    // Ŭ���� ���� �ܺο��� �б� ����
    public bool cleared { get; private set; } = false;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    /// <summary>
    /// �÷��̾ �� ������ ���� �� ȣ�� (DoorPortal����)
    /// </summary>
    public void OnPlayerEnter()
    {
        // ���� ���̰ų� �̹� Ŭ����� ���̸� �ƹ� �ϵ� ���� ����
        if (isStartRoom || cleared) return;

        // �� �ݰ�
        SetDoorsActive(false);

        // ���� ����
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

        // ���� ���� üũ ����
        StartCoroutine(CheckMonsters());
    }

    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            // ����(null) ���� ����
            spawnedMonsters.RemoveAll(x => x == null);

            if (spawnedMonsters.Count == 0)
            {
                // ���� �������� Ŭ����
                cleared = true;
                SetDoorsActive(true);
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Up/Down/Left/Right ���� �Ѳ����� �Ѱų� ���ϴ�.
    /// </summary>
    public void SetDoorsActive(bool on)
    {
        if (doorUp != null) doorUp.SetActive(on);
        if (doorDown != null) doorDown.SetActive(on);
        if (doorLeft != null) doorLeft.SetActive(on);
        if (doorRight != null) doorRight.SetActive(on);
    }
}
