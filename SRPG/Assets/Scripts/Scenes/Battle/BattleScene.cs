public class BattleScene : BaseScene
{
    protected override void Init()
    {
        _sceneType = Define.Scene.Battle;

        base.Init();

        Managers.scene.turn.CreateTurnbar();
        Managers.sound.Play("Battle", Define.Sound.BGM);
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
