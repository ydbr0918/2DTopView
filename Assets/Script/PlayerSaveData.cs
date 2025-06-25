using System;

[Serializable]
public class PlayerSaveData
{
    public int level;
    public int exp;
    public int expToNext;

    public int currentHp;
    public int maxHp;

    public int currentAmmo;
    public int maxAmmo;

    // 필요하면 추가 필드 선언
    // public float playerPosX, playerPosY;
}
