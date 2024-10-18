using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelUP : UIPopup
{
    public enum Images
    {
        Face,
        Cancle
    }

    public enum Texts
    {
        LevelText,
        StatText
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        BindUIEvent(GetImage((int)Images.Cancle).gameObject, (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void Setting(LevelUpTemp before, Player after)
    {
        GetImage((int)Images.Face).sprite = after.Face;
        GetText((int)Texts.LevelText).text = $"·¹º§ : {before.level}->{after.stat.Level}";
        GetText((int)Texts.StatText).text = $"AP : {before.ap}->{after.stat.AP}\r\nSP : {before.sp}->{after.stat.SP}";
    }

    public void CancleClick()
    {
        if(Managers.ui.TryClosePopupUI(this))
        {
            Managers.ui.ClosePopupUI();
        }
    }
}
