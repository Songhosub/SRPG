using UnityEngine.UI;

public class UI_Manual : UIPopup
{
    public enum Texts
    {
        Name,
        Effect
    }

    public void Init(string _name = "", string _effect = "")
    {
        base.Init();

        Bind<Text>(typeof(Texts));

        GetText((int)Texts.Name).text = _name;
        GetText((int)Texts.Effect).text = _effect;
    }
}
