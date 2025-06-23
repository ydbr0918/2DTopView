using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string weaponName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("ź��/��Ÿ��")]
    public int maxAmmo = 10;
    public float fireRate = 0.5f;  // �ʴ� �߻� ���� Ƚ�� = 1/fireRate
    public float reloadTime = 1f;    // ������ �ҿ� �ð�

    [Header("źȯ �Ӽ�")]
    public float bulletSpeed = 8f;
    public float range = 10f;   // ��Ÿ�
    public int baseDamage = 10;    // ���� ������

    [Header("���� ����")]
    public int bulletPerShot = 1;   // �� �� Ŭ�� �� �߻�Ǵ� �Ѿ� ����
    public float spreadAngle = 0f;  // ���� ����(������ ���� >0)

   
    public int damage => baseDamage;
}
