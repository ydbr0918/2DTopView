using UnityEngine;

[CreateAssetMenu(fileName = "AmmoBoostSkill", menuName = "Game/Skill/AmmoBoost")]
public class AmmoBoostSkill : SkillData
{
    public int bonusAmmo = 10;

    public override void Activate(GameObject user)
    {
        Weapon weapon = user.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.IncreaseMaxAmmo(bonusAmmo);
            Debug.Log("≈∫√¢ ¡ı∞°µ  + " + bonusAmmo);
        }
    }
}
