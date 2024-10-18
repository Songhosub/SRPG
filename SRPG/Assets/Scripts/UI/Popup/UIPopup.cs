public class UIPopup : UIBase
{
    public override void Init()
    {
        Managers.ui.SetCanvas(gameObject);
    }

    public virtual void ClosePopupUI()
    {
        Managers.ui.ClosePopupUI();
    }
}
