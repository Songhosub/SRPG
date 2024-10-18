using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameStart : UIPopup
{
    public enum Texts
    {
        NewGame,
        LoadGame
    }

    public enum GameObjects
    {
        Cancle
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        foreach (int i in Enum.GetValues(typeof(Texts)))
        {
            BindUIEvent(GetText(i).gameObject, (PointerEventData data) => { ClickDown(GetText(i)); }, Define.UIEvent.Over);
            BindUIEvent(GetText(i).gameObject, (PointerEventData data) => { ClickUp(GetText(i)); }, Define.UIEvent.Exit);
        }

        BindUIEvent(GetText((int)Texts.NewGame).gameObject, (PointerEventData data) => { NewGameClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetText((int)Texts.LoadGame).gameObject, (PointerEventData data) => { LoadGameClick(); }, Define.UIEvent.Click);

        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void ClickDown(Text _text)
    {
        _text.fontSize = 110;
    }

    public void ClickUp(Text _text)
    {
        _text.fontSize = 90;
    }

    public void NewGameClick()
    {
        Managers.data.StatDict.story = 0;
        Managers.data.StatDict.stats.Clear();
        Managers.scene.LoadScene("Dialog");
    }

    public void LoadGameClick()
    {
        Managers.ui.ShowPopupUI<UI_Load>().Init();
    }

    public void CancleClick()
    {
        Managers.ui.ClosePopupUI();
    }
}
