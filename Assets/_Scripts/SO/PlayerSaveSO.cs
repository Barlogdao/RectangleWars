using System.IO;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "NewPlayerSave", menuName = "ScriptableObjects/PlayerSave", order = 8)]
public class PlayerSaveSO : ScriptableObject
{
    public int SceneIndex;
    [ColorUsage (true, true)]
    public Color PlayerColor;
    public Complexity GameCompexity;
    public GamePlayersCondition Condition;
    public Hero hero;
    public Hero EnemyHero;
    private const string FILENAME = "PlayerSave.dat";

    public void SaveToFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        var json = JsonUtility.ToJson(this);
        File.WriteAllText(filePath, json);
    }

    public void LoadDataFromFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File \"{filePath}\" not found!", this);
            return;
        }
        var json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, this);
    }
    [Button]
    public void ResetSave()
    {
        SceneIndex = 0;
        hero = null;
        SaveToFile();
    }
}
