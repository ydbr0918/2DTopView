using UnityEngine;
using System.Collections;  // IEnumerator ������ �ʿ��մϴ�

[CreateAssetMenu(fileName = "HealSkill", menuName = "Game/Skill/Heal")]
public class HealSkill : SkillData
{
    [Header("Heal Skill Settings")]
    public int healAmount = 30;

    // ��ų ��ü�� �ڷ�ƾ ���¸� �����Ƿ�
    private bool isOnCooldown = false;

    public override void Activate(GameObject user)
    {
        // �̹� ��Ÿ�� ���̸� ���� �� ��
        if (isOnCooldown) return;

        // user���� Player ������Ʈ ��������
        Player player = user.GetComponent<Player>();
        if (player == null) return;

        // 1) ��� ȸ��
        player.Heal(healAmount);
        Debug.Log($"[HealSkill] HP ȸ�� +{healAmount}");

        // 2) �ڷ�ƾ���� ��Ÿ�� ���� (�÷��̾� �ʿ��� ����)
        player.StartCoroutine(CooldownCoroutine());
    }

    /// <summary>
    /// ��Ÿ�� ó���� �ڷ�ƾ
    /// </summary>
    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);  // SkillData���� ��ӵ� cooldown �ʵ�
        isOnCooldown = false;
    }
}
