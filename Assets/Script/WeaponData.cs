using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("기본 정보")]
    public string weaponName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("탄약/쿨타임")]
    public int maxAmmo = 10;
    public float fireRate = 0.5f;  // 초당 발사 가능 횟수 = 1/fireRate
    public float reloadTime = 1f;    // 재장전 소요 시간

    [Header("탄환 속성")]
    public float bulletSpeed = 8f;
    public float range = 10f;   // 사거리
    public int baseDamage = 10;    // 내부 데이터

    [Header("샷건 전용")]
    public int bulletPerShot = 1;   // 한 번 클릭 시 발사되는 총알 개수
    public float spreadAngle = 0f;  // 퍼짐 각도(샷건일 때만 >0)

   
    public int damage => baseDamage;
}
