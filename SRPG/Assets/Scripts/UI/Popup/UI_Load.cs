using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;

public class UI_Load : UIPopup
{
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
        Cancle
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<UI_Save_Slot>(typeof(SaveSlots));

        foreach (int i in Enum.GetValues(typeof(SaveSlots)))
        {
            Get<UI_Save_Slot>(i).Init();

            if (File.Exists($"{Application.persistentDataPath}/{Get<UI_Save_Slot>(i).gameObject.name}.Json"))
            {
                BindUIEvent(Get<UI_Save_Slot>(i).gameObject, (PointerEventData data) => { Load(i); }, Define.UIEvent.Click);
            }

            else if (Managers.resource.Load<TextAsset>("Data/" + Get<UI_Save_Slot>(i).gameObject.name) != null)
            {
                BindUIEvent(Get<UI_Save_Slot>(i).gameObject, (PointerEventData data) => { Load(i); }, Define.UIEvent.Click);
            }
        }

        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);

    }

    public void Load(int index)
    {
        Get<UI_Save_Slot>(index).Load();
    }

    public void CancleClick()
    {
        Managers.ui.ClosePopupUI();
    }
}
