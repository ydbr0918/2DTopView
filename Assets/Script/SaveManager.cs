using System.IO;
using UnityEngine;

public static class SaveManager
{

    private static string saveFilePath =>
        Path.Combine(Application.persistentDataPath, "save.json");


    public static SaveData LoadedData { get; private set; }

 
    public static bool HasSave => File.Exists(saveFilePath);


    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] 저장 완료: {saveFilePath}");
    }


    public static void LoadGame()
    {
        if (!HasSave) { Debug.LogWarning("불러올 세이브 없음"); return; }
        string json = File.ReadAllText(saveFilePath);
        LoadedData = JsonUtility.FromJson<SaveData>(json);

        var sel = SelectionData.Instance;
        sel.SetSelectedWeapon(LoadedData.selectedWeaponIndex);
        sel.SetSelectedSkill(LoadedData.selectedSkillIndex);

        Debug.Log("[SaveManager] 로드 완료");
    }



    public static void DeleteSave()
    {
        if (HasSave)
        {
            File.Delete(saveFilePath);
            Debug.Log($"[SaveManager] 세이브 삭제 완료: {saveFilePath}");
        }
    }
}
