using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GetCharacter : UIPopup
{
    List<Stat> stats = new List<Stat>();
    int currentIndex = 0;

    public enum Images
    {
        Face,
        Back_Image,
        Front_Image,
        Cancle
    }

    public enum Texts
    {
        HPText,
        AtkText,
        AtkDisText,
        MPText,
        AgiText,
        MovDisText
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Cancle).gameObject, (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click); 
    }

    public void Setting(List<Stat> _stats)
    {
        stats = _stats;
        currentIndex = 0;

        SetIndex();
    }

    public void PageBack()
    {
        currentIndex--;
        SetIndex();
    }

    public void PageFront()
    {
        currentIndex++;
        SetIndex();
    }

    public void SetIndex()
    {
        if (currentIndex == 0)
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.white;
        }

        if (currentIndex + 1 == stats.Count)
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.white;
        }

        GetUI(currentIndex);
    }

    public void GetUI(int index)
    {
        GetImage((int)Images.Face).sprite = Managers.resource.Load<Sprite>(stats[index].character, "Sprites/Face/");
        GetText((int)Texts.HPText).text = $"체력 : {stats[index].MaxHP}";
        GetText((int)Texts.AtkText).text = $"공격력 : {stats[index].Atk}";
        GetText((int)Texts.AtkDisText).text = $"공격거리 : {stats[index].AttackDis}";
        GetText((int)Texts.MPText).text = $"마나 : {stats[index].MaxMP}";
        GetText((int)Texts.AgiText).text = $"행동력 : {stats[index].Agi}";
        GetText((int)Texts.MovDisText).text = $"이동거리 : {stats[index].MoveDis}";
    }

    public void CancleClick()
    {
        Managers.scene.LoadScene($"Map{Managers.data.StatDict.story}");
    }
}
