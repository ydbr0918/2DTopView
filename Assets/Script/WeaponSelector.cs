using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject[] weapons;             // ���� ������Ʈ��
    public WeaponData[] weaponDatas;         // ���� ���� ScriptableObject�� (Inspector�� ����)

    private int currentIndex = 0;

    void Start()
    {
        ShowWeapon(currentIndex);  // ���� �� ù ���� ǥ�� + ���� ���� ����
    }

    public void ShowNextWeapon()
    {
        currentIndex = (currentIndex + 1) % weapons.Length;
        ShowWeapon(currentIndex);
    }

    public void ShowPreviousWeapon()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = weapons.Length - 1;

        ShowWeapon(currentIndex);
    }

    private void ShowWeapon(int index)
    {
        // 1. ���� ������Ʈ �����ֱ�
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // 2. ���� ���� �����ϱ�
        if (weaponDatas != null && weaponDatas.Length > index)
        {
            SelectionData.Instance.selectedWeapon = weaponDatas[index];
        }
        else
        {
            Debug.LogWarning("WeaponData�� ����Ǿ� ���� �ʰų� �ε��� ������ �߸��Ǿ����ϴ�.");
        }
    }
}
