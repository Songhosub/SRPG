using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Quit : UIPopup
{
    public enum GameObjects
    {
        OK,
        Cancle
    }


    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        BindUIEvent(GetGameObject((int)GameObjects.OK), (PointerEventData data) => { OKClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void OKClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CancleClick()
    {
        Managers.ui.ClosePopupUI();
    }
}
