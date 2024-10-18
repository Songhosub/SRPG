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
        GetText((int)Texts.Name_Text).text = $"�̸� : {stats[index].name}";
        GetText((int)Texts.Level_Text).text = $"���� : {stats[index].Level}";
        GetText((int)Texts.Status_Text_1).text = $"ü�� : {stats[index].MaxHP}\n���ݷ� : {stats[index].Atk}";
        GetText((int)Texts.Status_Text_2).text = $"���� : {stats[index].MaxMP}\n�ൿ�� : {stats[index].Agi}";
    }

    public void Clear()
    {
        GetImage((int)Images.Face).sprite = null;
        GetText((int)Texts.Name_Text).text = "�̸� : ";
        GetText((int)Texts.Level_Text).text = "���� : ";
        GetText((int)Texts.Status_Text_1).text = "ü�� : \n���ݷ� : ";
        GetText((int)Texts.Status_Text_2).text = "���� : \n�ൿ�� : ";
    }
}
