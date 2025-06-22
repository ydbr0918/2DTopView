// WeaponSelector.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 쓸 경우

public class WeaponSelector : MonoBehaviour
{
    [Header("무기 아이콘 (GameObject)")]
    public List<GameObject> weaponIcons;

    [Header("무기 데이터 (ScriptableObject)")]
    public List<WeaponData> weaponDataList;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void OnNextWeapon()
    {
        currentIndex = (currentIndex + 1) % weaponIcons.Count;
        UpdateUI();
    }

    public void OnPrevWeapon()
    {
        currentIndex = (currentIndex - 1 + weaponIcons.Count) % weaponIcons.Count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < weaponIcons.Count; i++)
            weaponIcons[i].SetActive(i == currentIndex);
    }

    /// <summary>
    /// “게임 시작” 버튼 등에 연결
    /// </summary>
    public void ConfirmSelection()
    {
        // 선택 저장
        SelectionData.Instance.SetSelectedWeapon(currentIndex, weaponDataList[currentIndex]);

        // 다음 씬 로드 (예시)
        // SceneManager.LoadScene("GameScene");
    }
}
