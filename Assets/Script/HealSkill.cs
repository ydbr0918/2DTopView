using UnityEngine;
using System.Collections;  // IEnumerator 쓰려면 필요합니다

[CreateAssetMenu(fileName = "HealSkill", menuName = "Game/Skill/Heal")]
public class HealSkill : SkillData
{
    [Header("Heal Skill Settings")]
    public int healAmount = 30;

    // 스킬 자체가 코루틴 상태를 가지므로
    private bool isOnCooldown = false;

    public override void Activate(GameObject user)
    {
        // 이미 쿨타임 중이면 동작 안 함
        if (isOnCooldown) return;

        // user에서 Player 컴포넌트 가져오기
        Player player = user.GetComponent<Player>();
        if (player == null) return;

        // 1) 즉시 회복
        player.Heal(healAmount);
        Debug.Log($"[HealSkill] HP 회복 +{healAmount}");

        // 2) 코루틴으로 쿨타임 시작 (플레이어 쪽에서 실행)
        player.StartCoroutine(CooldownCoroutine());
    }

    /// <summary>
    /// 쿨타임 처리용 코루틴
    /// </summary>
    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);  // SkillData에서 상속된 cooldown 필드
        isOnCooldown = false;
    }
}
