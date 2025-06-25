using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // ���� ȭ�鿡 ���� WeaponIcon �� currentIndex ��°�� �����ߴ�
        SelectionData.Instance.SetSelectedWeapon(currentIndex);

        // �� ��ȯ(��: SelectRoom �� TopViewMap_1) ȣ��
        SceneManager.LoadScene("TopViewMap_1");
    }
}
