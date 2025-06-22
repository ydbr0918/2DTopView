using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    public GameObject[] skills;             // ��ų ������Ʈ��
    public SkillData[] skillDatas;          // ��ų ���� ScriptableObject �迭 (Inspector���� ����)

    private int currentIndex = 0;

    void Start()
    {
        ShowSkill(currentIndex);            // ���� �� ù ��ų ǥ�� + ���� ���� ����
    }

    public void ShowNextSkill()
    {
        currentIndex = (currentIndex + 1) % skills.Length;
        ShowSkill(currentIndex);
    }

    public void ShowPreviousSkill()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = skills.Length - 1;

        ShowSkill(currentIndex);
    }

    private void ShowSkill(int index)
    {
        // 1. ��ų UI ǥ��
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(i == index);
        }

        // 2. ������ ��ų ������ ����
        if (skillDatas != null && skillDatas.Length > index)
        {
            SelectionData.Instance.selectedSkill = skillDatas[index];
        }
        else
        {
            Debug.LogWarning("SkillData�� ����Ǿ� ���� �ʰų� �ε��� ������ ������ϴ�.");
        }
    }
}
