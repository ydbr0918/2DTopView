using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [Header("아이콘들 (UI GameObject)")]
    public List<GameObject> weaponIcons;

    [Header("스크립터블 오브젝트 리스트")]
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
        // 전부 껐다가 현재 것만 켜기
        for (int i = 0; i < weaponIcons.Count; i++)
            weaponIcons[i].SetActive(i == currentIndex);
    }

    /// <summary>
    /// “게임 시작” 버튼의 OnClick 에 연결할 메서드
    /// </summary>
    public void ConfirmWeapon()
    {
        // 현재 인덱스에 대응하는 WeaponData 를 SelectionData 에 저장
        WeaponData data = weaponDataList[currentIndex];
        SelectionData.Instance.SetSelectedWeapon(currentIndex, data);

        // (필요하다면 여기서 씬 전환 호출)
        // e.g. SceneManager.LoadScene("GameScene");
    }
}
