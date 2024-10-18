using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby : UIScene
{
    UI_Player_Slot[] slots = new UI_Player_Slot[8];
    public List<Sprite> sprites = new List<Sprite>();
    public List<Stat> stats = new List<Stat>();
    int maxIndex = 0;
    int currnetIndex = 0;

    UI_Develop develop;
    UI_SkillTree skillTree;
    UI_Save save;
    UI_Option option;
    UI_Map map;

    public enum Images
    {
        Develop_Image,
        SkillTree_Image,
        Option_Image,
        Save_Image,
        Proceed_Image,
        Front_Image,
        Back_Image,
        Quit_Image
    }

    public enum Texts
    {
        Stage_Text,
        Index_Text
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        Transform slotsParent = Util.FindChild<GridLayoutGroup>(gameObject, "Player_List", true).transform;
        StatData statData = Managers.data.StatDict;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = Managers.ui.MakeSubItem<UI_Player_Slot>(slotsParent);
            slots[i].Init();
        }

        for (int i = 0; i < statData.stats.Count; i++) // ĳ���� ����ŭ �ݺ�
        {
            int index = i % slots.Length; //�ִ� ���� ������ �߰� �� �ʱ�ȭ
            stats.Add(statData.stats[i]);
            sprites.Add(Managers.resource.Load<Sprite>(statData.stats[i].character, "Sprites/Face/"));
            slots[index].stats.Add(stats[i]);
            slots[index].sprites.Add(sprites[i]);
            if (index == 0) //8���� ���� ������ �������� ����
            {
                maxIndex++;
            }
        }

        maxIndex = statData.stats.Count / slots.Length + 1;

        GetText((int)Texts.Stage_Text).text = $"Stage{Managers.data.StatDict.story}";

        SetIndex();

        BindUIEvent(GetImage((int)Images.Develop_Image).gameObject, (PointerEventData data) => { DevelopClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.SkillTree_Image).gameObject, (PointerEventData data) => { SkillTreeClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Option_Image).gameObject, (PointerEventData data) => { OptionClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Save_Image).gameObject, (PointerEventData data) => { SaveClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Proceed_Image).gameObject, (PointerEventData data) => { ProceedClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Quit_Image).gameObject, (PointerEventData data) => { Managers.scene.LoadScene("Start"); }, Define.UIEvent.Click);
    }

    public void PageBack()
    {
        currnetIndex--;
        SetIndex();
    }

    public void PageFront()
    {
        currnetIndex++;
        SetIndex();
    }

    public void SetIndex()
    {
        if(currnetIndex == 0) 
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.gray;
        } //ù �������� ��� Back ��ư�� ��Ȱ��ȭ
        else
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.white;
        } //�ƴ� ��� Back ��ư�� Ȱ��ȭ

        if (currnetIndex + 1 == maxIndex) 
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.gray;
        } //������ �������� ��� Fornt ��ư�� ��Ȱ��ȭ
        else
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.white;
        } //�ƴ� ��� Fornt ��ư�� Ȱ��ȭ

        GetText((int)Texts.Index_Text).text = $"{currnetIndex + 1} / {maxIndex}";

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].stats.Count > currnetIndex) //������ ������ �������� ���� ����
            {
                slots[i].GetComponent<Image>().color = Color.white;
                slots[i].Setting(currnetIndex);
            }
            else //�� ���� �˰� ǥ��
            {
                slots[i].GetComponent<Image>().color = Color.gray;
                slots[i].Clear();
            }
        }
    }

    public void DevelopClick()
    {
        BindUIEvent(GetImage((int)Images.Develop_Image).gameObject, (PointerEventData data) => { DevelopUnclick(); }, Define.UIEvent.Click);
        develop = Managers.ui.ShowPopupUI<UI_Develop>();
        develop.Init();
        develop.Setting(this);
        GetImage((int)Images.Develop_Image).color = Color.gray;
    }
    
    public void DevelopUnclick()
    {
        if(Managers.ui.TryClosePopupUI(develop))
        {
            BindUIEvent(GetImage((int)Images.Develop_Image).gameObject, (PointerEventData data) => { DevelopClick(); }, Define.UIEvent.Click);
            Managers.ui.ClosePopupUI();
            GetImage((int)Images.Develop_Image).color = Color.white;
        }
    }

    public void SkillTreeClick()
    {
        BindUIEvent(GetImage((int)Images.SkillTree_Image).gameObject, (PointerEventData data) => { SkillTreeUnclick(); }, Define.UIEvent.Click);
        skillTree = Managers.ui.ShowPopupUI<UI_SkillTree>();
        skillTree.Init();
        skillTree.Setting(this);
        GetImage((int)Images.SkillTree_Image).color = Color.gray;
    }

    public void SkillTreeUnclick()
    {
        if (Managers.ui.TryClosePopupUI(skillTree))
        {
            BindUIEvent(GetImage((int)Images.SkillTree_Image).gameObject, (PointerEventData data) => { SkillTreeClick(); }, Define.UIEvent.Click);
            Managers.ui.ClosePopupUI();
            GetImage((int)Images.SkillTree_Image).color = Color.white;
        }
    }

    public void OptionClick()
    {
        BindUIEvent(GetImage((int)Images.Option_Image).gameObject, (PointerEventData data) => { OptionUnclick(); }, Define.UIEvent.Click);
        option = Managers.ui.ShowPopupUI<UI_Option>();
        option.Init();
        option.Setting(this);
        GetImage((int)Images.Option_Image).color = Color.gray;
    }

    public void OptionUnclick()
    {
        if (Managers.ui.TryClosePopupUI(option))
        {
            BindUIEvent(GetImage((int)Images.Option_Image).gameObject, (PointerEventData data) => { OptionClick(); }, Define.UIEvent.Click);
            Managers.ui.ClosePopupUI();
            GetImage((int)Images.Option_Image).color = Color.white;
        }
    }

    public void SaveClick()
    {
        BindUIEvent(GetImage((int)Images.Save_Image).gameObject, (PointerEventData data) => { SaveUnclick(); }, Define.UIEvent.Click);
        save = Managers.ui.ShowPopupUI<UI_Save>();
        save.Init();
        save.Setting(this);
        GetImage((int)Images.Save_Image).color = Color.gray;
    }

    public void SaveUnclick()
    {
        if (Managers.ui.TryClosePopupUI(save))
        {
            BindUIEvent(GetImage((int)Images.Save_Image).gameObject, (PointerEventData data) => { SaveClick(); }, Define.UIEvent.Click);
            Managers.ui.ClosePopupUI();
            GetImage((int)Images.Save_Image).color = Color.white;
        }
    }

    public void ProceedClick()
    {
        BindUIEvent(GetImage((int)Images.Proceed_Image).gameObject, (PointerEventData data) => { ProceedUnclick(); }, Define.UIEvent.Click);
        map = Managers.ui.ShowPopupUI<UI_Map>();
        map.Init();
        map.Setting(this);
        GetImage((int)Images.Proceed_Image).color = Color.gray;
    }

    public void ProceedUnclick()
    {
        if (Managers.ui.TryClosePopupUI(map))
        {
            BindUIEvent(GetImage((int)Images.Proceed_Image).gameObject, (PointerEventData data) => { ProceedClick(); }, Define.UIEvent.Click);
            Managers.ui.ClosePopupUI();
            GetImage((int)Images.Proceed_Image).color = Color.white;
        }
    }

    public void Cheet()
    {
        foreach(Stat stat in stats)
        {
            stat.Level += 100;
            stat.AP += 100;
            stat.SP += 100;
        }
        SetIndex();
    }
}
