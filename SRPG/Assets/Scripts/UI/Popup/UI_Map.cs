using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Map : UIPopup
{
    UI_Lobby Lobby;

    public enum Images
    {
        Stage0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    }

    public enum Texts
    {
        Stage_Text
    }

    public enum GameObjects
    {
        Panel,
        OK,
        Cancle,
        Close
    }

    public override void Init()
    {
        base.Init();
        
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.Panel).SetActive(false);

        foreach (int i in Enum.GetValues(typeof(Images)))
        {
            if(i <= Managers.data.StatDict.story)
            {
                GetImage(i).color = Color.white;
                BindUIEvent(GetImage(i).gameObject, 
                    (PointerEventData data) => { StageClick(i); }, Define.UIEvent.Click);
            }
        }

        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Close), (PointerEventData data) => { CloseClick(); }, Define.UIEvent.Click);
    }

    public void Setting(UI_Lobby _Lobby)
    {
        Lobby = _Lobby;
    }

    public void StageClick(int i)
    {
        Get<GameObject>((int)GameObjects.Panel).SetActive(true);
        GetText((int)Texts.Stage_Text).text = $"Stage{i + 1}";
        BindUIEvent(GetGameObject((int)GameObjects.OK), (PointerEventData data) => { OKClick(i); }, Define.UIEvent.Click);
    }

    public void OKClick(int i)
    {
        if(i == Managers.data.StatDict.story)
        {
            Managers.scene.LoadScene("Dialog");
        }

        else
        {
            Managers.scene.LoadScene($"Map{i}");
        }
    }

    public void CancleClick()
    {
        Get<GameObject>((int)GameObjects.Panel).SetActive(false);
    }

    public void CloseClick()
    {
        Lobby.ProceedUnclick();
    }
}
