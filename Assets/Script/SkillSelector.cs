using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("아이콘들 (UI GameObject)")]
    public List<GameObject> skillIcons;

    [Header("스크립터블 오브젝트 리스트")]
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
    /// “게임 시작” 버튼의 OnClick 에 이 메서드를 연결하세요.
    /// </summary>
    public void ConfirmSkill()
    {
        SelectionData.Instance.SetSelectedSkill(currentIndex);
    }
}
