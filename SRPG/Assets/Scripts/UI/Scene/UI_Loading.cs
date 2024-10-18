using UnityEngine.UI;

public class UI_Loading : UIScene
{
    public enum Sliders
    {
        Slider
    }

    public enum Texts
    {
        Text
    }

    public override void Init()
    {
        base.Init();
        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));
    }

    public Slider ReturnSlider()
    {
        return Get<Slider>((int)Sliders.Slider);
    }

    public Text ReturnText()
    {
        return GetText((int)Texts.Text);
    }
}
