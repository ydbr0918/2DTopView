using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [Header("�����ܵ� (UI GameObject)")]
    public List<GameObject> weaponIcons;

    [Header("��ũ���ͺ� ������Ʈ ����Ʈ")]
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
        // ���� ���ٰ� ���� �͸� �ѱ�
        for (int i = 0; i < weaponIcons.Count; i++)
            weaponIcons[i].SetActive(i == currentIndex);
    }

    /// <summary>
    /// ������ ���ۡ� ��ư�� OnClick �� ������ �޼���
    /// </summary>
    public void ConfirmWeapon()
    {
        // ���� �ε����� �����ϴ� WeaponData �� SelectionData �� ����
        WeaponData data = weaponDataList[currentIndex];
        SelectionData.Instance.SetSelectedWeapon(currentIndex, data);

        // (�ʿ��ϴٸ� ���⼭ �� ��ȯ ȣ��)
        // e.g. SceneManager.LoadScene("GameScene");
    }
}
