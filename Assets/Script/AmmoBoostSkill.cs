using UnityEngine;

[CreateAssetMenu(fileName = "AmmoBoostSkill", menuName = "Game/Skill/AmmoBoost")]
public class AmmoBoostSkill : SkillData
{
    [Header("탄창 보너스")]
    public int bonusAmmo = 10;

    public override void Activate(GameObject user)
    {
        var player = user.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("[AmmoBoostSkill] Activate 실패: Player 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        player.IncreaseMaxAmmo(bonusAmmo);
        Debug.Log($"[AmmoBoostSkill] 탄창 증가 +{bonusAmmo}");
    }
}
