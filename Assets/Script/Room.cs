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

    [HideInInspector] public bool isStartRoom = false;

    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool cleared = false;

    public void OnPlayerEnter()
    {
        if (isStartRoom || cleared) return;

        SetDoorsActive(false);
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
                SetDoorsActive(true);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetDoorsActive(bool active)
    {
        if (doorUp != null) doorUp.SetActive(active);
        if (doorDown != null) doorDown.SetActive(active);
        if (doorLeft != null) doorLeft.SetActive(active);
        if (doorRight != null) doorRight.SetActive(active);
    }
}
