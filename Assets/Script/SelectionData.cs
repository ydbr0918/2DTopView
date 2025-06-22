using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static SelectionData Instance;

    public WeaponData selectedWeapon;
    public SkillData selectedSkill;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
