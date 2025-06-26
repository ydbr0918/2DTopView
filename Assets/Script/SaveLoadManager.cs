using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string fileName = "playerSave.json";
    private static string FilePath => Path.Combine(Application.persistentDataPath, fileName);


    public static void Save(PlayerSaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(FilePath, json);
        Debug.Log($"[Save] 저장 완료 → {FilePath}");
    }


    public static PlayerSaveData Load()
    {
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("[Load] 저장 파일이 없습니다.");
            return null;
        }

        string json = File.ReadAllText(FilePath);
        var data = JsonUtility.FromJson<PlayerSaveData>(json);
        Debug.Log($"[Load] 불러오기 완료: Level={data.level}, HP={data.currentHp}/{data.maxHp}");
        return data;
    }
}
