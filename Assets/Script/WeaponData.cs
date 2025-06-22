using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public int damage;
    public int maxAmmo;

    public GameObject weaponPrefab; // ���� ���� ������ (�Ѿ� �߻� ��)
}
