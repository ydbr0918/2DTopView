using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentIndex = 0;

    void Start()
    {
        ShowWeapon(currentIndex);
    }

    public void ShowNextWeapon()
    {
        currentIndex = (currentIndex + 1) % weapons.Length;
        ShowWeapon(currentIndex);
    }

    private void ShowWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }
    }
    public void ShowPreviousWeapon()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = weapons.Length - 1;

        ShowWeapon(currentIndex);
    }

}
