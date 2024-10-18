using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Slot : UIBase
{
    public List<Stat> stats = new List<Stat>();
    public List<Sprite> sprites = new List<Sprite>();

    public enum Images
    {
        Face
    }

    public enum Texts
    {
        Name_Text,
        Level_Text,
        Status_Text_1,
        Status_Text_2
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        stats.Clear();
    }

    public void Setting(int index)
    {
        GetImage((int)Images.Face).sprite = sprites[index];
        GetText((int)Texts.Name_Text).text = $"이름 : {stats[index].name}";
        GetText((int)Texts.Level_Text).text = $"레벨 : {stats[index].Level}";
        GetText((int)Texts.Status_Text_1).text = $"체력 : {stats[index].MaxHP}\n공격력 : {stats[index].Atk}";
        GetText((int)Texts.Status_Text_2).text = $"마나 : {stats[index].MaxMP}\n행동력 : {stats[index].Agi}";
    }

    public void Clear()
    {
        GetImage((int)Images.Face).sprite = null;
        GetText((int)Texts.Name_Text).text = "이름 : ";
        GetText((int)Texts.Level_Text).text = "레벨 : ";
        GetText((int)Texts.Status_Text_1).text = "체력 : \n공격력 : ";
        GetText((int)Texts.Status_Text_2).text = "마나 : \n행동력 : ";
    }
}
