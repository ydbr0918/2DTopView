using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableVector2Int
{
    public int x, y;
    public SerializableVector2Int(int x, int y) { this.x = x; this.y = y; }
    public Vector2Int ToVector2Int() => new Vector2Int(x, y);
}

[Serializable]
public class SaveData
{
    // 플레이어 상태
    public int selectedWeaponIndex;
    public int selectedSkillIndex;
    public int level;
    public int exp;
    public int maxHp;
    public int currentHp;
    public int maxAmmo;
    public int currentAmmo;
    public int savedBaseDamage;      // 스킬로 증가된 플레이어 기본 데미지
    public float savedFireRate;      // 무기 발사 속도
    public float savedBulletSpeed;   // (필요하다면) 탄환 속도

    // 맵 상태
    public List<SerializableVector2Int> roomPositions;
    public SerializableVector2Int currentRoomPos;
    public List<SerializableVector2Int> clearedRooms;

 
}
