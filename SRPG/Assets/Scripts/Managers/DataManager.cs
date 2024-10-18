using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    public StatData GetDict = new StatData();
    public StatData StatDict = new StatData();
    public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();

    public T LoadJson<T>(string path) where T : class
    {
        if (File.Exists($"{Application.persistentDataPath}/{path}.Json"))
        {
            path = $"{Application.persistentDataPath}/{path}.Json";
            string saveFile = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(saveFile);
        }

        else if (Resources.Load<TextAsset>($"Data/{path}") != null)
        {
            path = $"Data/{path}";
            string saveFile = Resources.Load<TextAsset>(path).ToString();
            return JsonUtility.FromJson<T>(saveFile);
        }

        return null;
    }

    public void SaveJson<T>(T saveData,string path) where T : class
    {
        path = $"{Application.persistentDataPath}/{path}.Json";

        string saveJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, saveJson);
    }
    
    public void RenewerStat(Stat _stat)
    {
        for(int i = 0; i < StatDict.stats.Count; i++)
        {
            if (StatDict.stats[i].name == _stat.name)
            {
                StatDict.stats[i] = _stat;
            }
        }
    }
    
    public void Init()
    {
        //StatData의 종류에 따라 다양한 타입의 Dictionary에 담을 수 있음
        GameObject go = Managers.resource.Load<GameObject>("Skill", "Prefabs/");
        foreach (Skill skill in go.GetComponents<Skill>())
        {
            skill.Setting();
            skills.Add(skill.Name, skill);
        }
    }
}