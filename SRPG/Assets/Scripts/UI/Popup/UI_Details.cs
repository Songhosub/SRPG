using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Details : UIPopup
{
    public enum Images
    {
        BackGround,
        Panel,
        Icon,
        Skill1_Icon,
        Skill2_Icon,
        Skill3_Icon,
        Cancle
    }

    public enum Texts
    {
        Name,
        Level,
        Skill1_Text,
        Skill2_Text,
        Skill3_Text,
        Exp,
        HP,
        MP,
        Atk,
        Agi
    }

    public void Setting(Character character)
    {
        Init();

        float a = GetImage((int)Images.Panel).color.a;

        if (character.GetComponent<Player>() != null)
        {
            GetImage((int)Images.Panel).color = new Color(0, 0, 1, 0.75f);
            GetText((int)Texts.Exp).text = $"����ġ : {character.stat.Exp} / {character.NextLevelExp}";
        }
        else
        {
            GetImage((int)Images.Panel).color = new Color(1, 0, 0, 0.75f);
            GetText((int)Texts.Exp).text = $"ȹ�� ����ġ : {character.stat.Exp}";
        }

        GetImage((int)Images.Icon).sprite = character.Face;

        for(int i = 0; i < 3; i++)
        {
            if (i < character.skills.Count)
            {
                GetImage((int)Images.Skill1_Icon + i).sprite = character.skills[i].Icon;
                GetText((int)Texts.Skill1_Text + i).text = character.skills[i].ManualName();
            }

            else
            {
                GetImage((int)Images.Skill1_Icon + i).gameObject.SetActive(false);
                GetText((int)Texts.Skill1_Text + i).text = "";
            }
        }

        GetText((int)Texts.Name).text = character.stat.name;
        GetText((int)Texts.Level).text = "���� : " + character.stat.Level;
        GetText((int)Texts.HP).text = $"ü�� : {character.HP} / {character.stat.MaxHP}";
        GetText((int)Texts.MP).text = $"���� : {character.MP} / {character.stat.MaxMP}";
        GetText((int)Texts.Atk).text = "���ݷ� : " + character.stat.Atk;
        GetText((int)Texts.Agi).text = "�ൿ�� : " + character.stat.Agi;

        BindUIEvent(GetImage((int)Images.BackGround).gameObject, (PointerEventData data) => { Managers.ui.ClosePopupUI(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Cancle).gameObject, (PointerEventData data) => { Managers.ui.ClosePopupUI(); }, Define.UIEvent.Click);
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
    }

    public void SlotSetting(Character character)
    {
        GetImage((int)Images.BackGround).gameObject.SetActive(false);
        GetImage((int)Images.Cancle).gameObject.SetActive(false);

        GetText((int)Texts.HP).text = $"ü�� : {character.stat.MaxHP} / {character.stat.MaxHP}";
        GetText((int)Texts.MP).text = $"���� : {character.stat.MaxMP} / {character.stat.MaxMP}";
    }
}
