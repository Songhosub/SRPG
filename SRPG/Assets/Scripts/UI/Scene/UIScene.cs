public class UIScene : UIBase
{
    public override void Init()
    {
        Managers.ui.SetCanvas(gameObject, false);
    }
}
