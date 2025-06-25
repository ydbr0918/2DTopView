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
    // �÷��̾� ����
    public int selectedWeaponIndex;
    public int selectedSkillIndex;
    public int level;
    public int exp;
    public int maxHp;
    public int currentHp;
    public int maxAmmo;
    public int currentAmmo;
    public int savedBaseDamage;      // ��ų�� ������ �÷��̾� �⺻ ������
    public float savedFireRate;      // ���� �߻� �ӵ�
    public float savedBulletSpeed;   // (�ʿ��ϴٸ�) źȯ �ӵ�

    // �� ����
    public List<SerializableVector2Int> roomPositions;
    public SerializableVector2Int currentRoomPos;
    public List<SerializableVector2Int> clearedRooms;

 
}
