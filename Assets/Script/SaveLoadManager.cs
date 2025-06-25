using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string fileName = "playerSave.json";
    private static string FilePath => Path.Combine(Application.persistentDataPath, fileName);

    /// <summary>
    /// JSON으로 직렬화해 파일로 저장
    /// </summary>
    public static void Save(PlayerSaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(FilePath, json);
        Debug.Log($"[Save] 저장 완료 → {FilePath}");
    }

    /// <summary>
    /// 저장 파일이 있으면 JSON을 역직렬화해 반환,
    /// 없으면 null
    /// </summary>
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
