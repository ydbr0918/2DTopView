using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    public GameObject[] skills;             // 스킬 오브젝트들
    public SkillData[] skillDatas;          // 스킬 정보 ScriptableObject 배열 (Inspector에서 연결)

    private int currentIndex = 0;

    void Start()
    {
        ShowSkill(currentIndex);            // 시작 시 첫 스킬 표시 + 선택 정보 저장
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
        // 1. 스킬 UI 표시
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(i == index);
        }

        // 2. 선택한 스킬 데이터 저장
        if (skillDatas != null && skillDatas.Length > index)
        {
            SelectionData.Instance.selectedSkill = skillDatas[index];
        }
        else
        {
            Debug.LogWarning("SkillData가 연결되어 있지 않거나 인덱스 범위를 벗어났습니다.");
        }
    }
}
