using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    public GameObject[] skills;
    private int currentIndex = 0;

    void Start()
    {
        ShowSkill(currentIndex);
    }

    public void ShowNextSkill()
    {
        currentIndex = (currentIndex + 1) % skills.Length;
        ShowSkill(currentIndex);
    }

    private void ShowSkill(int index)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(i == index);
        }
    }
    public void ShowPreviousSkill()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = skills.Length - 1;

        ShowSkill(currentIndex);
    }

}
