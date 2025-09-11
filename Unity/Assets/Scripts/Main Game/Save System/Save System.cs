using System.IO;
using UnityEngine;

public class SaveSystem
{
    public string path = Application.persistentDataPath + "/score.json";
    public void SaveScore(int hiScore)
    {
        ScoreData data = new ScoreData();
        data.hiScore = hiScore;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public int LoadScore()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            return data.hiScore;
        }
        return 0;
    }
}
