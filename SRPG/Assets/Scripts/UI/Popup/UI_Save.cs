using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Save : UIPopup
{
    UI_Lobby Lobby;

    public enum SaveSlots
    {
        Save01,
        Save02,
        Save03,
        Save04,
        Save05,
        Save06,
        Save07,
        Save08,
        Save09,
        Save10,
        Save11,
        Save12,
        Save13,
        Save14,
        Save15,
        Save16,
        Save17,
        Save18,
        Save19,
        Save20
    }

    public enum GameObjects
    {
        Cancle,
        Panel
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<UI_Save_Slot>(typeof(SaveSlots));

        foreach (int i in Enum.GetValues(typeof(SaveSlots)))
        {
            Get<UI_Save_Slot>(i).Init();
            BindUIEvent(Get<UI_Save_Slot>((int)i).gameObject, (PointerEventData data) => { Save((int)i); }, Define.UIEvent.Click);
        }

        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);

        GetGameObject((int)GameObjects.Panel).SetActive(false);
    }

    public void Setting(UI_Lobby _Lobby)
    {
        Lobby = _Lobby;
    }

    public void Save(int index)
    {
        Get<UI_Save_Slot>(index).Save();
        StartCoroutine(Noti());
    }

    public void CancleClick()
    {
        Lobby.SaveUnclick();
    }

    IEnumerator Noti()
    {
        GetGameObject((int)GameObjects.Panel).SetActive(true);
        yield return new WaitForSeconds(1);
        GetGameObject((int)GameObjects.Panel).SetActive(false);
    }
}
