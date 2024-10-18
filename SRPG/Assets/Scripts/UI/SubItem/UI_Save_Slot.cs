using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_Save_Slot : UIBase
{
    Image image;
    StatData data;

    public enum Texts
    {
        Save_Num,
        Character_Num,
        Stage
    }

    public override void Init()
    {
        image = GetComponent<Image>();
        Bind<Text>(typeof(Texts));

        if (File.Exists($"{Application.persistentDataPath}/{gameObject.name}.Json"))
        {
            image.color = Color.gray;
            data = Managers.data.LoadJson<StatData>(gameObject.name);
            GetText((int)Texts.Character_Num).text = $"캐릭터 수 : {data.stats.Count}";
            GetText((int)Texts.Stage).text = $"스케이지 진행도 : {data.story}";
            GetText((int)Texts.Save_Num).text = gameObject.name;
        }

        else if (Managers.resource.Load<TextAsset>("Data/" + gameObject.name) != null)
        {
            image.color = Color.gray;
            data = Managers.data.LoadJson<StatData>(gameObject.name);
            GetText((int)Texts.Character_Num).text = $"캐릭터 수 : {data.stats.Count}";
            GetText((int)Texts.Stage).text = $"스케이지 진행도 : {data.story}";
            GetText((int)Texts.Save_Num).text = gameObject.name;
        }
    }

    public void Save()
    {
        image.color = Color.gray;
        GetText((int)Texts.Character_Num).text = $"캐릭터 수 : {Managers.data.StatDict.stats.Count}";
        GetText((int)Texts.Stage).text = $"스케이지 진행도 : {Managers.data.StatDict.story}";
        GetText((int)Texts.Save_Num).text = gameObject.name;
        Managers.data.SaveJson(Managers.data.StatDict, gameObject.name);
    }

    public void Load()
    {
        Managers.data.StatDict = data;
        Managers.scene.LoadScene("Lobby");
    }
}
