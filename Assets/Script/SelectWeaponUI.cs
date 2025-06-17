using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponUI : MonoBehaviour
{
    public Sprite[] weaponSprites;       // ���� �̹��� ���
    public Image weaponImage;            // ���� �����ִ� UI
    private int currentWeaponIndex = 0;  // ���� ���õ� ���� �ε���

    // ��ư���� ȣ���� �Լ�
    public void ShowNextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weaponSprites.Length)
            currentWeaponIndex = 0;

        weaponImage.sprite = weaponSprites[currentWeaponIndex];
    }
}
