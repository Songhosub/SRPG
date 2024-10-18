public class StartScene : BaseScene
{
    protected override void Init()
    {
        _sceneType = Define.Scene.Start;

        base.Init();

        Managers.ui.ShowSceneUI<UI_Start>().Init();
        Managers.sound.Play("Start", Define.Sound.BGM);
    }

    private void Awake()
    {
        Init();
    }

    public override void Clear()
    {
        base.Clear();
    }
}
