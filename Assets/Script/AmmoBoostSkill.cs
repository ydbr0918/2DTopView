using UnityEngine;

[CreateAssetMenu(fileName = "AmmoBoostSkill", menuName = "Game/Skill/AmmoBoost")]
public class AmmoBoostSkill : SkillData
{
    [Header("źâ ���ʽ�")]
    public int bonusAmmo = 10;

    public override void Activate(GameObject user)
    {
        var player = user.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("[AmmoBoostSkill] Activate ����: Player ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }
        player.IncreaseMaxAmmo(bonusAmmo);
        Debug.Log($"[AmmoBoostSkill] źâ ���� +{bonusAmmo}");
    }
}
