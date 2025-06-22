using UnityEngine;

[CreateAssetMenu(fileName = "HealSkill", menuName = "Game/Skill/Heal")]
public class HealSkill : SkillData
{
    public int healAmount = 30;
    private bool isOnCooldown = false;

    public override void Activate(GameObject user)
    {
        if (isOnCooldown) return;

        PlayerHealth health = user.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log("HP »∏∫π: +" + healAmount);
        }

        user.GetComponent<MonoBehaviour>().StartCoroutine(StartCooldown());
    }

    private System.Collections.IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown); // 20√ 
        isOnCooldown = false;
    }
}
