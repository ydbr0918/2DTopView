using System.IO;
using UnityEngine;

public static class SaveManager
{
    // �� ���⿡ ��� �ʵ�(Ȥ�� ������Ƽ)�� �����ؾ� Path, File.* ȣ���� �����մϴ�.
    private static string saveFilePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    // LoadGame() ���� ä������ ������
    public static SaveData LoadedData { get; private set; }

    // ���� ���� ���� üũ
    public static bool HasSave => File.Exists(saveFilePath);

    // �� ������ ����
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] ���� �Ϸ�: {saveFilePath}");
    }

    // �� ������ �ε�
    public static void LoadGame()
    {
        if (!HasSave) { Debug.LogWarning("�ҷ��� ���̺� ����"); return; }
        string json = File.ReadAllText(saveFilePath);
        LoadedData = JsonUtility.FromJson<SaveData>(json);

        // �� ���õ� ���⡤��ų ����
        var sel = SelectionData.Instance;
        sel.SetSelectedWeapon(LoadedData.selectedWeaponIndex);
        sel.SetSelectedSkill(LoadedData.selectedSkillIndex);

        Debug.Log("[SaveManager] �ε� �Ϸ�");
    }


    // �� ���̺� ���� (�� ���� ���� �� ȣ��)
    public static void DeleteSave()
    {
        if (HasSave)
        {
            File.Delete(saveFilePath);
            Debug.Log($"[SaveManager] ���̺� ���� �Ϸ�: {saveFilePath}");
        }
    }
}
