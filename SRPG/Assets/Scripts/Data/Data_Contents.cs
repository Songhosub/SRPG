using System;
using System.Collections.Generic;

#region Stat
[Serializable]
public class Stat
{
    public string character;
    public string name;
    public int weaponType;

    public int Level = 1;
    public int Exp = 0;

    public int MaxHP = 100;
    public int MaxMP = 100;

    public int Atk = 10;
    public int Agi = 1; //턴이 오는 속도
    public int MoveDis = 3; //이동 거리
    public int AttackDis = 1; //공격 거리
    public int UseSkillNum = 1;
    public List<string> Skills = new List<string>();

    public int AP = 0;
    public int SP = 0;
}

[Serializable]
public class Dialog
{
    public int speakerIndex;        //다이얼로그의 Speaker의 배열 순번
    public string speakerName;             //캐릭터 이름
    public string spritePath;
    public string speakerDialog;           //대사
}

[Serializable]
public class StatData : ILoader<string, Stat>
{
    public int story;
    public List<Stat> stats = new List<Stat>();

    public Dictionary<string, Stat> MakeDict()
    {
        Dictionary<string, Stat> dict = new Dictionary<string, Stat>();
        foreach (Stat stat in stats)
        {
            dict.Add(stat.character, stat);
        }

        return dict;
    }
}

[Serializable]
public class DialogData
{
    public List<Dialog> dialog = new List<Dialog>();
}

#endregion
