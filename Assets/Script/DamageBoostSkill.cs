using UnityEngine;

[CreateAssetMenu(fileName = "DamageBoostSkill", menuName = "Game/Skill/DamageBoost")]
public class DamageBoostSkill : SkillData
{
    public float bonusDamage = 10f;

    public override void Activate(GameObject user)
    {
        Weapon weapon = user.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.IncreaseDamage(bonusDamage);
            Debug.Log("무기 데미지 증가됨 + " + bonusDamage);
        }
    }
}
