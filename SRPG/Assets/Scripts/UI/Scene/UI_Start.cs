using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class UI_Start : UIScene
{
    Resolution res;

    public enum Texts
    {
        GameStart,
        Option,
        Quit
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));

        foreach (int i in Enum.GetValues(typeof(Texts))) //각 텍스트들이 마우스가 놓이면 커지도록 설정
        {
            BindUIEvent(GetText(i).gameObject, (PointerEventData data) => { ClickDown(GetText(i)); }, Define.UIEvent.Over);
            BindUIEvent(GetText(i).gameObject, (PointerEventData data) => { ClickUp(GetText(i)); }, Define.UIEvent.Exit);
        }

        BindUIEvent(GetText((int)Texts.GameStart).gameObject, (PointerEventData data) => { GameStartClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetText((int)Texts.Option).gameObject, (PointerEventData data) => { OptionClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetText((int)Texts.Quit).gameObject, (PointerEventData data) => { Quit(); }, Define.UIEvent.Click);

        res = Camera.main.GetOrAddComponent<Resolution>();
        res.ResolutionSet(1280, 720);
    }

    public void ClickDown(Text _text)
    {
        _text.fontSize = 120;
    }

    public void ClickUp(Text _text)
    {
        _text.fontSize = 100;
    }

    public void GameStartClick()
    {
        Managers.ui.ShowPopupUI<UI_GameStart>().Init();
    }

    public void OptionClick()
    {
        Managers.ui.ShowPopupUI<UI_Option>().Init();
    }

    public void Quit()
    {
        Managers.ui.ShowPopupUI<UI_Quit>().Init();
    }
}
