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
        Debug.Log($"[Save] ���� �Ϸ� �� {FilePath}");
    }


    public static PlayerSaveData Load()
    {
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("[Load] ���� ������ �����ϴ�.");
            return null;
        }

        string json = File.ReadAllText(FilePath);
        var data = JsonUtility.FromJson<PlayerSaveData>(json);
        Debug.Log($"[Load] �ҷ����� �Ϸ�: Level={data.level}, HP={data.currentHp}/{data.maxHp}");
        return data;
    }
}
