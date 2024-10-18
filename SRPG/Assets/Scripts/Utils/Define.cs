public class Define
{
    public enum GameoverType
    {
        Win,
        Lose
    }

    public enum SearchType
    {
        Move,
        Attack
    }

    public enum UIEvent
    {
        Drag,
        Click,
        Over,
        Exit,
    }

    public enum Scene
    {
        Unknown,
        Start,
        Lobby,
        Battle,
        Loading,
        Dialog
    }

    public enum Sound
    {
        Master,
        BGM,
        Effect
    }

    public enum WeaponType
    {
        Sword,
        Claw
    }
}