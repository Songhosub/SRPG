using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Develop : UIPopup
{
    UI_Lobby Lobby;
    List<Stat> stats = new List<Stat>();
    List<Sprite> sprites = new List<Sprite>();
    int currentIndex = 0;
    int currentAP = 0;
    int currentHP = 0;
    int currentMP = 0;
    int currentAtk = 0;
    int currentAgi = 0;

    public enum Images
    {
        Face,
        Front_Image,
        Back_Image
    }

    public enum Texts
    {
        Name_Text,
        Level_Text,
        AP_Text,
        HP_Text,
        MP_Text,
        Atk_Text,
        Agi_Text,
        Index_Text
    }

    public enum GameObjects
    {
        OK,
        Cancle,
        HP_Down,
        HP_Up,
        MP_Down,
        MP_Up,
        Atk_Down,
        Atk_Up,
        Agi_Down,
        Agi_Up,
        Panel
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetGameObject((int)GameObjects.Panel).SetActive(false);

        BindUIEvent(GetGameObject((int)GameObjects.HP_Up), (PointerEventData data) => { HPUpClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.HP_Down), (PointerEventData data) => { HPDownClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.MP_Up), (PointerEventData data) => { MPUpClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.MP_Down), (PointerEventData data) => { MPDownClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Atk_Up), (PointerEventData data) => { AtkUpClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Atk_Down), (PointerEventData data) => { AtkDownClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Agi_Up), (PointerEventData data) => { AgiUpClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Agi_Down), (PointerEventData data) => { AgiDownClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.OK), (PointerEventData data) => { OKClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void GetUI(int index)
    {
        GetImage((int)Images.Face).sprite = sprites[index];
        GetText((int)Texts.Name_Text).text = stats[index].name;
        GetText((int)Texts.Level_Text).text = $"Lv.{stats[index].Level}";
        GetText((int)Texts.AP_Text).text = $"AP : {stats[index].AP}";
        GetText((int)Texts.HP_Text).text = $"체력 : {stats[index].MaxHP}";
        GetText((int)Texts.MP_Text).text = $"마나 : {stats[index].MaxMP}";
        GetText((int)Texts.Atk_Text).text = $"공격력 : {stats[index].Atk}";
        GetText((int)Texts.Agi_Text).text = $"행동력 : {stats[index].Agi}";
        GetText((int)Texts.Index_Text).text = $"{index + 1} / {stats.Count}";

        currentAP = stats[index].AP;
        currentHP = stats[index].MaxHP;
        currentMP = stats[index].MaxMP;
        currentAtk = stats[index].Atk;
        currentAgi = stats[index].Agi;
    }

    public void Setting(UI_Lobby _Lobby)
    {
        Lobby = _Lobby;
        stats = Lobby.stats;
        sprites = Lobby.sprites;
        currentIndex = 0;
        SetIndex();
    }

    public void HPUpClick()
    {
        if (currentAP > 0)
        {
            currentAP--;
            currentHP += 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.HP_Text).text = $"체력 : {currentHP}";
        }
    }

    public void HPDownClick()
    {
        if (currentHP > stats[currentIndex].MaxHP)
        {
            currentAP++;
            currentHP -= 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.HP_Text).text = $"체력 : {currentHP}";
        }
    }

    public void MPUpClick()
    {
        if (currentAP > 0)
        {
            currentAP--;
            currentMP += 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.MP_Text).text = $"마나 : {currentMP}";
        }
    }

    public void MPDownClick()
    {
        if (currentMP > stats[currentIndex].MaxMP)
        {
            currentAP++;
            currentMP -= 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.MP_Text).text = $"마나 : {currentMP}";
        }
    }

    public void AtkUpClick()
    {
        if (currentAP > 0)
        {
            currentAP--;
            currentAtk += 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.Atk_Text).text = $"공격력 : {currentAtk}";
        }
    }

    public void AtkDownClick()
    {
        if (currentAtk > stats[currentIndex].Atk)
        {
            currentAP++;
            currentAtk -= 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.Atk_Text).text = $"공격력 : {currentAtk}";
        }
    }

    public void AgiUpClick()
    {
        if (currentAP > 0)
        {
            currentAP--;
            currentAgi += 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.Agi_Text).text = $"행동력 : {currentAgi}";
        }
    }

    public void AgiDownClick()
    {
        if (currentAgi > stats[currentIndex].Agi)
        {
            currentAP++;
            currentAgi -= 50;
            GetText((int)Texts.AP_Text).text = $"AP : {currentAP}";
            GetText((int)Texts.Agi_Text).text = $"행동력 : {currentAgi}";
        }
    }

    public void OKClick()
    {
        stats[currentIndex].AP = currentAP;
        stats[currentIndex].MaxHP = currentHP;
        stats[currentIndex].MaxMP = currentMP;
        stats[currentIndex].Atk = currentAtk;
        stats[currentIndex].Agi = currentAgi;

        StartCoroutine(Noti());
        //Managers.data.RenewerStat(stats[currentIndex]);
    }

    public void CancleClick()
    {
        Lobby.DevelopUnclick();
    }

    IEnumerator Noti()
    {
        GetGameObject((int)GameObjects.Panel).SetActive(true);
        yield return new WaitForSeconds(1);
        GetGameObject((int)GameObjects.Panel).SetActive(false);
    }

    public void PageBack()
    {
        currentIndex--;
        SetIndex();
    }

    public void PageFront()
    {
        currentIndex++;
        SetIndex();
    }

    public void SetIndex()
    {
        if (currentIndex == 0)
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.white;
        }

        if (currentIndex + 1 == stats.Count)
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.white;
        }

        GetUI(currentIndex);
    }
}
