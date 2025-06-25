using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("�����ܵ� (UI GameObject)")]
    public List<GameObject> skillIcons;

    [Header("��ũ���ͺ� ������Ʈ ����Ʈ")]
    public List<SkillData> skillDataList;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void OnNextSkill()
    {
        currentIndex = (currentIndex + 1) % skillIcons.Count;
        UpdateUI();
    }

    public void OnPrevSkill()
    {
        currentIndex = (currentIndex - 1 + skillIcons.Count) % skillIcons.Count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < skillIcons.Count; i++)
            skillIcons[i].SetActive(i == currentIndex);
    }

    /// <summary>
    /// ������ ���ۡ� ��ư�� OnClick �� �� �޼��带 �����ϼ���.
    /// </summary>
    public void ConfirmSkill()
    {
        SelectionData.Instance.SetSelectedSkill(currentIndex);
    }
}
