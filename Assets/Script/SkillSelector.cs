// SkillSelector.cs
using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("스킬 아이콘 (GameObject)")]
    public List<GameObject> skillIcons;

    [Header("스킬 데이터 (ScriptableObject)")]
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
    /// “게임 시작” 버튼 등에 연결
    /// </summary>
    public void ConfirmSelection()
    {
        // 선택 저장
        SelectionData.Instance.SetSelectedSkill(currentIndex, skillDataList[currentIndex]);

        // (씬 전환은 WeaponSelector 쪽과 동일하게 하셔도 되고)
    }
}
