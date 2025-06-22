// SkillSelector.cs
using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("��ų ������ (GameObject)")]
    public List<GameObject> skillIcons;

    [Header("��ų ������ (ScriptableObject)")]
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
    /// ������ ���ۡ� ��ư � ����
    /// </summary>
    public void ConfirmSelection()
    {
        // ���� ����
        SelectionData.Instance.SetSelectedSkill(currentIndex, skillDataList[currentIndex]);

        // (�� ��ȯ�� WeaponSelector �ʰ� �����ϰ� �ϼŵ� �ǰ�)
    }
}
