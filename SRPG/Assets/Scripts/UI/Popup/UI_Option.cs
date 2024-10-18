using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Option : UIPopup
{
    UI_Lobby Lobby;
    Resolution res;

    public enum Texts
    {
        MasterVolume_Text,
        BGMVolume_Text,
        EffectVolume_Text
    }

    public enum Scrollbars
    {
        MasterVolume_Scrollbar,
        BGMVolume_Scrollbar,
        EffectVolume_Scrollbar
    }

    public enum Dropdowns
    {
        Resolution_Dropdown
    }
    
    public enum GameObjects
    {
        Template,
        Cancle
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Scrollbar>(typeof(Scrollbars));
        Bind<TMP_Dropdown>(typeof(Dropdowns));
        Bind<GameObject>(typeof(GameObjects));


        foreach(int i in Enum.GetValues(typeof(Scrollbars)))
        {
            Scrollbar scrollbar = Get<Scrollbar>(i);
            scrollbar.value = Managers.sound.Volumes[i];
            GetText(i).text = $"{(int)(scrollbar.value * 100)}%";
            scrollbar.onValueChanged.AddListener( delegate { Managers.sound.VolumeSetting((Define.Sound)(i), scrollbar, GetText(i)); });
        }

        TMP_Dropdown Resolution_Dropdown = Get<TMP_Dropdown>((int)Dropdowns.Resolution_Dropdown);
        Resolution_Dropdown.onValueChanged.AddListener(delegate { ResolutionSetting(Resolution_Dropdown); });

        res = Camera.main.GetOrAddComponent<Resolution>();

        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void Setting(UI_Lobby _Lobby)
    {
        Lobby = _Lobby;
    }

    public void ResolutionSetting(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                res.ResolutionSet(1280, 720);
                break;
            case 1:
                res.ResolutionSet(1920, 1080);
                break;
            case 2:
                res.ResolutionSet(2560, 1440);
                break;
            default:
                break;
        }
    }

    public void CancleClick()
    {
        if(Managers.scene.CurrentScene._sceneType == Define.Scene.Lobby)
        {
            Lobby.OptionUnclick();
        }

        else
        {
            Managers.ui.ClosePopupUI();
        }
    }
}
