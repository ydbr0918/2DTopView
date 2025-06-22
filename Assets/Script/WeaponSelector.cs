// WeaponSelector.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� �� ���

public class WeaponSelector : MonoBehaviour
{
    [Header("���� ������ (GameObject)")]
    public List<GameObject> weaponIcons;

    [Header("���� ������ (ScriptableObject)")]
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
    /// ������ ���ۡ� ��ư � ����
    /// </summary>
    public void ConfirmSelection()
    {
        // ���� ����
        SelectionData.Instance.SetSelectedWeapon(currentIndex, weaponDataList[currentIndex]);

        // ���� �� �ε� (����)
        // SceneManager.LoadScene("GameScene");
    }
}
