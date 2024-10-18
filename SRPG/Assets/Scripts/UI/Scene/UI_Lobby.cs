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

        for (int i = 0; i < statData.stats.Count; i++) // 캐릭터 수만큼 반복
        {
            int index = i % slots.Length; //최대 슬롯 수까지 추가 후 초기화
            stats.Add(statData.stats[i]);
            sprites.Add(Managers.resource.Load<Sprite>(statData.stats[i].character, "Sprites/Face/"));
            slots[index].stats.Add(stats[i]);
            slots[index].sprites.Add(sprites[i]);
            if (index == 0) //8명을 넘을 때마다 페이지가 증가
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
        } //첫 페이지일 경우 Back 버튼을 비활성화
        else
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.white;
        } //아닐 경우 Back 버튼을 활성화

        if (currnetIndex + 1 == maxIndex) 
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.gray;
        } //마지막 페이지일 경우 Fornt 버튼을 비활성화
        else
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.white;
        } //아닐 경우 Fornt 버튼을 활성화

        GetText((int)Texts.Index_Text).text = $"{currnetIndex + 1} / {maxIndex}";

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].stats.Count > currnetIndex) //슬롯의 정보를 페이지에 맞춰 변경
            {
                slots[i].GetComponent<Image>().color = Color.white;
                slots[i].Setting(currnetIndex);
            }
            else //빈 슬롯 검게 표시
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
