using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
 
        for (int i = 0; i < weaponIcons.Count; i++)
            weaponIcons[i].SetActive(i == currentIndex);
    }



    public void ConfirmWeapon()
    {
    
        SelectionData.Instance.SetSelectedWeapon(currentIndex);

      
        SceneManager.LoadScene("TopViewMap_1");
    }
}
