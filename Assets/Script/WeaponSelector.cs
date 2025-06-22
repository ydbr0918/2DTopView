using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject[] weapons;             // 무기 오브젝트들
    public WeaponData[] weaponDatas;         // 무기 정보 ScriptableObject들 (Inspector에 연결)

    private int currentIndex = 0;

    void Start()
    {
        ShowWeapon(currentIndex);  // 시작 시 첫 무기 표시 + 선택 정보 저장
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
        // 1. 무기 오브젝트 보여주기
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // 2. 무기 정보 저장하기
        if (weaponDatas != null && weaponDatas.Length > index)
        {
            SelectionData.Instance.selectedWeapon = weaponDatas[index];
        }
        else
        {
            Debug.LogWarning("WeaponData가 연결되어 있지 않거나 인덱스 범위가 잘못되었습니다.");
        }
    }
}
