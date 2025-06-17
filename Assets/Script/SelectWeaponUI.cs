using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponUI : MonoBehaviour
{
    public Sprite[] weaponSprites;       // 무기 이미지 목록
    public Image weaponImage;            // 무기 보여주는 UI
    private int currentWeaponIndex = 0;  // 현재 선택된 무기 인덱스

    // 버튼에서 호출할 함수
    public void ShowNextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weaponSprites.Length)
            currentWeaponIndex = 0;

        weaponImage.sprite = weaponSprites[currentWeaponIndex];
    }
}
