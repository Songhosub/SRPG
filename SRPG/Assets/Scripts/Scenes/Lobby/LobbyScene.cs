public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        _sceneType = Define.Scene.Lobby;

        base.Init();

        Managers.ui.ShowSceneUI<UI_Lobby>().Init();
        Managers.sound.Play("Lobby", Define.Sound.BGM);
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
