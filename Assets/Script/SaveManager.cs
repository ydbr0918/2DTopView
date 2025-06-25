using System.IO;
using UnityEngine;

public static class SaveManager
{
    // ★ 여기에 경로 필드(혹은 프로퍼티)를 선언해야 Path, File.* 호출이 가능합니다.
    private static string saveFilePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    // LoadGame() 이후 채워지는 데이터
    public static SaveData LoadedData { get; private set; }

    // 저장 파일 유무 체크
    public static bool HasSave => File.Exists(saveFilePath);

    // ▶ 데이터 저장
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] 저장 완료: {saveFilePath}");
    }

    // ▶ 데이터 로드
    public static void LoadGame()
    {
        if (!HasSave) { Debug.LogWarning("불러올 세이브 없음"); return; }
        string json = File.ReadAllText(saveFilePath);
        LoadedData = JsonUtility.FromJson<SaveData>(json);

        // ① 선택된 무기·스킬 복원
        var sel = SelectionData.Instance;
        sel.SetSelectedWeapon(LoadedData.selectedWeaponIndex);
        sel.SetSelectedSkill(LoadedData.selectedSkillIndex);

        Debug.Log("[SaveManager] 로드 완료");
    }


    // ▶ 세이브 삭제 (새 게임 시작 시 호출)
    public static void DeleteSave()
    {
        if (HasSave)
        {
            File.Delete(saveFilePath);
            Debug.Log($"[SaveManager] 세이브 삭제 완료: {saveFilePath}");
        }
    }
}
