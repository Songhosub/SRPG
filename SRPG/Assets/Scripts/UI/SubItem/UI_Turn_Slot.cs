using UnityEngine;
using UnityEngine.UI;

public class UI_Turn_Slot : UIBase
{
    public enum Images
    {
        Image
    }

    public enum Texts
    {
        CurrentTimeText,
        MaxTimeText,
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
    }

    public void CreateSlot(int font)
    {
        Init();
        GetText((int)Texts.CurrentTimeText).fontSize = font;
        GetText((int)Texts.MaxTimeText).fontSize = font;
    }

    public void SetSlot(Sprite sprite, float currentText, float maxText)
    { 
        GetImage((int)Images.Image).sprite = sprite;
        if(currentText >= 1000)
        {
            GetText((int)Texts.CurrentTimeText).text = "999+";
        }
        else
        {
            GetText((int)Texts.CurrentTimeText).text = $"{Mathf.RoundToInt(currentText)}";
        }

        if (maxText >= 1000)
        {
            GetText((int)Texts.MaxTimeText).text = "999+";
        }
        else
        {
            GetText((int)Texts.MaxTimeText).text = $"{Mathf.RoundToInt(maxText)}";
        }
    }
}
